using DrawCurve.Application.Config;
using DrawCurve.Application.Hubs;
using DrawCurve.Application.Interface;
using DrawCurve.Application.Logger;
using DrawCurve.Application.Menedgers;
using DrawCurve.Application.Menedgers.Renders;
using DrawCurve.Application.Services;
using DrawCurve.Application.Utils;
using DrawCurve.Core.Window;
using DrawCurve.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DrawCurve.Application
{
    public static class ApplicationService
    {
        public static IServiceCollection AddRenderServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddRenderServices<TickRenderHub>(configuration);
        }
        public static IServiceCollection AddRenderServices<THub>(this IServiceCollection services, IConfiguration configuration) where THub : class, ISendTickRender
        {
            services.AddDbContext<DrawCurveDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            );
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IRenderQueue, RenderService>();
            services.AddScoped<IRenderService, RenderService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IVideoService, VideoService>();

            services.AddScoped<CheckLiminters>();
            services.AddScoped<FFMpegService>();

            services.AddScoped<ISendTickRender, THub>();

            services.AddSingleton<MenedgerConfig>();

            services.AddSingleton<MenedgerGenerateFrames>();
            services.AddSingleton<MenedgerConcatFrame>();
            services.AddSingleton<MenedgerVideoConcatAudio>();

            services.AddHostedService<MenedgerRenderHostedService>();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddProvider(new CustomLoggerProvider(new CustomLoggerConfiguration()));
            });

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

            services.Configure<RenderApplicationConfig>(options =>
                configuration.GetSection("RenderApplicationConfig"));

            configuration.ConfigureCore();

            return services;
        }

        private static void ConfigureCore(this IConfiguration cnf)
        {
            SvgCurveRender.Step = int.Parse(cnf.GetSection("RenderApplicationConfig:RenderSVG:IndexError").Value ?? SvgCurveRender.Step.ToString());
            SvgCurveRender.Components = int.Parse(cnf.GetSection("RenderApplicationConfig:RenderSVG:CountSegments").Value ?? SvgCurveRender.Components.ToString());
        }

    }
}
