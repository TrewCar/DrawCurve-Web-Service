using DrawCurve.Application.Hubs;
using DrawCurve.Application.Interface;
using DrawCurve.Application.Logger;
using DrawCurve.Application.Menedgers;
using DrawCurve.Application.Menedgers.Renders;
using DrawCurve.Application.Services;
using DrawCurve.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DrawCurve.Application
{
    public static class ApplicationService
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddApplicationServices<TickRenderHub>(configuration);
        }
        public static IServiceCollection AddApplicationServices<THub>(this IServiceCollection services, IConfiguration configuration) where THub : class, ISendTickRender
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

            return services;
        }
    }
}
