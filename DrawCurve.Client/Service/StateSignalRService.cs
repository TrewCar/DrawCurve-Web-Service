using Blazored.LocalStorage;
using DrawCurve.Domen.Responces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

public class StateSignalRService
{
    private HubConnection? _hubConnection;
    private HttpClient httpClient;
    private ILocalStorageService _localStorage;
    private NavigationManager navigation;
    private ILogger<StateSignalRService> _logger;

    public Action<RenderTick> Action { get; set; }

    public StateSignalRService(HttpClient httpClient, ILocalStorageService localStorage, NavigationManager navigation, ILogger<StateSignalRService> logger)
    {
        this.httpClient = httpClient;
        this._localStorage = localStorage;
        this.navigation = navigation;
        this._logger = logger;

        Init();
    }

    private void Init()
    {
        this._hubConnection = new HubConnectionBuilder()
            .WithUrl(navigation.ToAbsoluteUri(httpClient.BaseAddress + "_renderinfo"), options =>
            {
                options.AccessTokenProvider = async () => await _localStorage.GetItemAsync<string>("authToken");
            })
            .ConfigureLogging(logging =>
            {
                logging.SetMinimumLevel(LogLevel.Information);
                //logging.AddConsole();
            })
            .Build();

        _logger.LogInformation("Инициализация HubConnection завершена.");
    }

    public async Task Start()
    {
        if (this._hubConnection == null) { Init(); }

        _hubConnection.On<RenderTick>("tick", (obj) =>
        {
            Action?.Invoke(obj);
        });

        _hubConnection.Closed += async (error) =>
        {
            _logger.LogError("Соединение закрыто с ошибкой: {Error}", error?.Message);
            await Task.Delay(5000); // Подождем 5 секунд перед повторным подключением
            _logger.LogInformation("Повторное подключение...");
            await _hubConnection.StartAsync();
        };

        await _hubConnection.StartAsync();
        
        _logger.LogInformation("SignalR соединение установлено.");
    }

    public void Stop()
    {
        _logger.LogInformation("Остановка SignalR соединения...");
        Action = null;
        _hubConnection?.DisposeAsync();
        _hubConnection = null;
        _logger.LogInformation("Соединение остановлено.");
    }
}
