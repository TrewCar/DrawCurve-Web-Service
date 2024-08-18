using DrawCurve.Application.Interface;
using DrawCurve.Application.Menedgers;
using DrawCurve.Application.Services;
using DrawCurve.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddSingleton<MenedgerConfig>();
            services.AddSingleton<MenedgerRender>();

            services.AddHostedService<MenedgerRenderHostedService>();

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

            return services;
        }
    }
}
