using WebApiTemplate.Application.Interfaces;
using WebApiTemplate.Filters;
using WebApiTemplate.Infrastructure.Db;
using WebApiTemplate.Infrastructure.Security;
using WebApiTemplate.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// 建立控制器並將全域例外過濾器加入管線，確保錯誤輸出一致。
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();
});

// Swagger 提供互動式 API 說明，方便測試與溝通。
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 註冊資料庫設定與相關相依物件。
builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection(DatabaseOptions.SectionName));
builder.Services.AddSingleton<SqlConnectionFactory>();

// 註冊安全性相關元件。
builder.Services.AddSingleton<PasswordHasher>();
builder.Services.AddSingleton<JwtTokenFactory>();

// 註冊驗證服務的實作。
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
