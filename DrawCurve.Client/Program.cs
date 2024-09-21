using Blazored.LocalStorage;
using DrawCurve.Client;
using DrawCurve.Client.Provider;
using DrawCurve.Client.Provider.DrawCurve.Client.Provider;
using DrawCurve.Client.Service;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;

public static class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddAuthorizationCore();
        builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<RenderService>();
        builder.Services.AddScoped<StateSignalRService>();
        builder.Services.AddBlazoredLocalStorage();


        builder.Services.AddScoped(sp => new HttpClient
        {
            BaseAddress = new Uri("http://192.168.0.200:5184")
        });
        await builder.Build().RunAsync();
    }
}