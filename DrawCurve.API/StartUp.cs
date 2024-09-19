
using DrawCurve.API.Menedgers;
using DrawCurve.Application;
using DrawCurve.Application.Logger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
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

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });

            services.AddDistributedMemoryCache();

            services.AddHttpContextAccessor();

            services.AddScoped<JwtManager>();

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

            app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication(); // Включение аутентификации
            app.UseAuthorization();  // Включение авторизации

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // Настройка маршрутизации контроллеров
            });
        }
    }
}
