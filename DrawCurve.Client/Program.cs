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

        // Используем Startup для конфигурации
        var startup = new Startup();
        startup.ConfigureServices(builder.Services);
        startup.Configure(builder);

        await builder.Build().RunAsync();
    }
}
