using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DrawCurve.Application.Menedgers;
using Microsoft.Extensions.DependencyInjection;

namespace DrawCurve.Application
{
    public static class ApplicationService
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<MenedgerConfig>();

            return services;
        }
    }
}
