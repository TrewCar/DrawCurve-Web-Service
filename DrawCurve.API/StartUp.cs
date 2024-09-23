using DrawCurve.API.Menedgers;
using DrawCurve.Application;
using DrawCurve.Application.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddEndpointsApiExplorer();
            services.AddSignalR();

            services.AddSwaggerGen();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder => builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .SetIsOriginAllowed(origin => true)
                        .AllowCredentials());
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
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero // убираем стандартное время отклонения (5 минут)
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var path = context.HttpContext.Request.Path;

                            var accessToken = context.Request.Query["access_token"];

                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/_renderinfo"))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddDistributedMemoryCache();

            services.AddHttpContextAccessor();

            services.AddScoped<JwtManager>();

            services.AddApplicationServices(Configuration);
        }

        // Метод для настройки HTTP-пайплайна
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHubContext<TickRenderHub> hubContext)
        {
            HubHelper.Initialize(hubContext);
            if (env.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            } else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseSession();
            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // Включение аутентификации
            app.UseAuthorization();  // Включение авторизации

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<TickRenderHub>("/_renderinfo");
                endpoints.MapControllers(); // Настройка маршрутизации контроллеров
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
