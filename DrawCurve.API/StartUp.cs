﻿using DrawCurve.API.Hubs;
using DrawCurve.API.Menedgers;
using DrawCurve.Application;
using DrawCurve.Application.Logger;
using Microsoft.OpenApi.Models;
namespace DrawCurve.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // Метод для регистрации сервисов
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSignalR();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Some API v1", Version = "v1" });
                // here some other configurations maybe...
                options.AddSignalRSwaggerGen();
            });


            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Время жизни сессии
                options.Cookie.HttpOnly = true; // Сессии только через HTTP (нельзя через JavaScript)
                options.Cookie.IsEssential = true; // Куки сессий обязательны
            });

            services.AddDistributedMemoryCache();

            services.AddHttpContextAccessor();

            services.AddScoped<MenedgerSession>();

            services.AddApplicationServices(Configuration);
        }

        // Метод для настройки HTTP-пайплайна
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseSession();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // Настройка маршрутизации контроллеров
                endpoints.MapHub<SendRenderTickHub>("/tickrender");
            });
        }
    }
}