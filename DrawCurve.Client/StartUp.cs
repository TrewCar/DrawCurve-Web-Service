using Blazored.LocalStorage;
using DrawCurve.Client;
using DrawCurve.Client.Provider;
using DrawCurve.Client.Provider.DrawCurve.Client.Provider;
using DrawCurve.Client.Service;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Добавляем необходимые сервисы
        services.AddAuthorizationCore();
        services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

        services.AddScoped<UserService>();
        services.AddScoped<AuthService>();
        services.AddScoped<RenderService>();
        services.AddScoped<StateSignalRService>();

        services.AddBlazoredLocalStorage();

        // HttpClient конфигурация
        services.AddScoped(sp => new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5184")
        });
    }

    public void Configure(WebAssemblyHostBuilder builder)
    {
        // Логирование уровня Debug
        builder.Logging.SetMinimumLevel(LogLevel.Debug);

        // Подключаем компоненты
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        // Отключаем Browser Link
#if DEBUG
        // Если используется Browser Link, мы явно не подключаем его в проекте.
#endif
    }
}
