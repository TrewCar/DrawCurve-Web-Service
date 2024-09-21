using DrawCurve.Application;
using DrawCurve.Application.Interface;
using DrawCurve.Application.Menedgers.Renders;
using DrawCurve.Core.Window;
using DrawCurve.Domen.DTO.Models;
using DrawCurve.Domen.Models;
using DrawCurve.Domen.Models.Core.Objects;
using DrawCurve.Domen.Models.Menedger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello World");
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                // Устанавливаем конфигурацию из файла appsettings.json
                config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                config.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
            })
           .ConfigureServices((context, services) =>
           {
               services.AddApplicationServices(context.Configuration);
           })
           .Build();

        var scope = host.Services.CreateScope();

        var renders = scope.ServiceProvider.GetRequiredService<IRenderService>();
        //RenderInfo info = new RenderInfo()
        //{
        //    KEY = Guid.NewGuid().ToString(),
        //    AuthorId = 1,
        //    Type = RenderType.RenderCurve,
        //    Status = TypeStatus.ProccessInQueue,
        //    Name = "Test",
        //    Objects = new List<ObjectRender>
        //    {
        //        new LineCurve()
        //        {
        //            RPS = MathF.PI,
        //            Angle = 0,
        //            Length = 100
        //        },
        //        new LineCurve()
        //        {
        //            RPS = -MathF.PI / 3,
        //            Angle = 0,
        //            Length = 100
        //        },
        //    },
        //    RenderConfig = new CurveRender().GetDefaultRenderConfig().Transfer(),
        //    DateCreate = DateTime.Now
        //};

        //renders.Queue(info);
        var menedger1 = scope.ServiceProvider.GetRequiredService<MenedgerGenerateFrames>();
        var menedger2 = scope.ServiceProvider.GetRequiredService<MenedgerConcatFrame>();
        var menedger3 = scope.ServiceProvider.GetRequiredService<MenedgerVideoConcatAudio>();



        while (true)
        {

        }
    }

    public class ResiveMsg : ISendTickRender
    {
        public Task SendTick(int AuthorId, RenderTick tick)
        {
            return Console.Out.WriteLineAsync(tick.KeyRender + "\t" + tick.Status + "\t" + tick.CountFPS + "\\" + tick.MaxCountFPS + "\t\t" + tick.FPS);
        }
    }
}