using DrawCurve.API.Menedgers;
using DrawCurve.Application;
using DrawCurve.Application.Menedgers;
using Newtonsoft.Json;
internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30); // ����� ����� ������
            options.Cookie.HttpOnly = true; // ������ ������ ����� HTTP (������ ����� JavaScript)
            options.Cookie.IsEssential = true; // ���� ������ �����������
        });
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddHttpContextAccessor(); // ����������� ��� ������� � ��������� HTTP
        builder.Services.AddScoped<MenedgerSession>();

        builder.Services.AddApplicationServices(builder.Configuration);

        var app = builder.Build();
                        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseSession();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}