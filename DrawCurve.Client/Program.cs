using Blazored.LocalStorage;
using DrawCurve.Client;
using DrawCurve.Client.Provider;
using DrawCurve.Client.Provider.DrawCurve.Client.Provider;
using DrawCurve.Client.Service;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.Logging.SetMinimumLevel(LogLevel.Debug);

        // Подключаем компоненты
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddScoped(sp => new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5184")
            //BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
        });

        builder.Services.AddAuthorizationCore();
        builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<RenderService>();
        builder.Services.AddScoped<MusicService>();
        builder.Services.AddScoped<StateSignalRService>();

        builder.Services.AddBlazoredLocalStorage();

        await builder.Build().RunAsync();
    }
}
