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
        public static readonly string KeyConfig = "RenderConfig";
        public static readonly string KeyFFMpegPath = "FFMpegPath";
        public static readonly string KeyRenderSvg = "RenderSVG";
        public static readonly string KeyRenderSvg_Segments = "CountSegments";
        public static readonly string KeyRenderSvg_IndexError = "IndexError";
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

            services.ConfigurePathFFMpeg(configuration);
            services.ConfigureRenderSVG(configuration);

            return services;
        }
        /// <summary>
        /// Задание параметра для пути
        /// </summary>
        /// <param name="configuration">Path to start FFMpeg or only FFMpeg 
        /// <br/> If not use this function -> default code gen
        /// <code>Path.Combine(Directory.GetParent(Environment.ProcessPath).FullName, "Utils", "ffmpeg.exe");</code>
        /// <br/>
        ///  Or use "default" to use default gen
        ///  <br/>
        ///  Key = RenderConfig__FFMpegPath</param>
        public static void ConfigurePathFFMpeg(this IServiceCollection services, IConfiguration configuration)
        {
            string? path = configuration.GetSection(KeyConfig).GetSection(KeyFFMpegPath).Value ?? null;

            if (string.IsNullOrWhiteSpace(path) || path.ToLower() == "default")
                return;

            FFMpegUtils.PathToFFMpeg = path;
        }

        public static void ConfigureRenderSVG(this IServiceCollection services, IConfiguration configuration)
        {
            var cnfRenderSvg = configuration.GetSection(KeyConfig).GetSection(KeyRenderSvg);

            int CountSegment = int.Parse(cnfRenderSvg.GetSection(KeyRenderSvg_Segments).Value ?? "-1");
            double IndexError = double.Parse(cnfRenderSvg.GetSection(KeyRenderSvg_IndexError).Value ?? "-1");

            if (CountSegment > 0)
                SvgCurveRender.Components = CountSegment;

            if(IndexError > 0)
                SvgCurveRender.Step = IndexError;
        }
    }
}
