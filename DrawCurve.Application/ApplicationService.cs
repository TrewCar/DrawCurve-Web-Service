using DrawCurve.Application.Interface;
using DrawCurve.Application.Logger;
using DrawCurve.Application.Menedgers;
using DrawCurve.Application.Menedgers.Renders;
using DrawCurve.Application.Services;
using DrawCurve.Application.Utils;
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
            services.AddDbContext<DrawCurveDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            );
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IRenderQueue, RenderService>();
            services.AddScoped<IRenderService, RenderService>();
            services.AddScoped<IUserService, UserService>();

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
