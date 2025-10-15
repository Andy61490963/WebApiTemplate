using WebApiTemplate.Application.Interfaces;
using WebApiTemplate.Filters;
using WebApiTemplate.Infrastructure.Db;
using WebApiTemplate.Infrastructure.Security;
using WebApiTemplate.Infrastructure.Services;

namespace WebApiTemplate;

/// <summary>
/// 應用程式進入點。
/// </summary>
/// <remarks>Remark: 設定 DI、Swagger 與中介軟體管線。</remarks>
public class Program
{
    /// <summary>
    /// 主函式。
    /// </summary>
    /// <param name="args">命令列參數。</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder);

        var app = builder.Build();

        ConfigurePipeline(app);

        app.Run();
    }

    /// <summary>
    /// 註冊應用程式所需服務。
    /// </summary>
    /// <param name="builder">Web 應用程式產生器。</param>
    /// <remarks>Remark: 包含 MVC、Swagger 以及自訂相依性。</remarks>
    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ApiExceptionFilter>();
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<SqlConnectionFactory>();
        builder.Services.AddSingleton<JwtTokenFactory>();
        builder.Services.AddSingleton<PasswordHasher>();
        builder.Services.AddScoped<IAuthService, AuthService>();
    }

    /// <summary>
    /// 設定 HTTP 處理管線。
    /// </summary>
    /// <param name="app">Web 應用程式。</param>
    /// <remarks>Remark: 啟用 Swagger、HTTPS 與路由。</remarks>
    private static void ConfigurePipeline(WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
    }
}
