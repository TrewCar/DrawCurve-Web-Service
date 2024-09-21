using Blazored.LocalStorage;
using DrawCurve.Domen.Responces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace DrawCurve.Client.Service
{
    public class StateSignalRService
    {
        private HubConnection? _hubConnection;
        private HttpClient httpClient;
        private ILocalStorageService _localStorage;
        private NavigationManager navigation;

        public Action<RenderTick> Action { get; set; }

        public StateSignalRService(HttpClient httpClient, ILocalStorageService localStorage, NavigationManager navigation)
        {
            this.httpClient = httpClient;
            this._localStorage = localStorage;
            this.navigation = navigation;

            Init();
        }
        private void Init()
        {
            this._hubConnection = new HubConnectionBuilder()
                .WithUrl(navigation.ToAbsoluteUri(httpClient.BaseAddress + "tickRender"), options =>
                {
                    // Убираем .Result и делаем вызов асинхронным
                    options.AccessTokenProvider = async () => await _localStorage.GetItemAsync<string>("authToken");

                }).Build();
        }
        public async Task Start()
        {
            if(this._hubConnection == null) { Init(); }
            await _hubConnection.StartAsync();
            _hubConnection.On<RenderTick>("SendTick", (obj) => Action?.Invoke(obj));
        }
        public void Stop()
        {
            Action = null;
            _hubConnection?.DisposeAsync();
            _hubConnection = null;
        }
    }
}
