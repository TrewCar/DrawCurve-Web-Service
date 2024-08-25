using DrawCurve.API;
using DrawCurve.API.Menedgers;
using DrawCurve.Application;
using DrawCurve.Application.Menedgers;
using Newtonsoft.Json;
public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}