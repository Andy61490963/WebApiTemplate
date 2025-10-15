namespace WebApiTemplate.Infrastructure.Services;

using WebApiTemplate.Application.DTOs.Auth;
using WebApiTemplate.Application.Interfaces;
using WebApiTemplate.Infrastructure.Db;
using WebApiTemplate.Infrastructure.Security;

/// <summary>
/// 負責整合資料庫、密碼雜湊與 JWT 產生邏輯的服務骨架。
/// </summary>
/// <remarks>
/// 目前僅提供方法定義與依賴注入，實際查詢將使用 Dapper 於後續實作中完成。
/// </remarks>
public sealed class AuthService : IAuthService
{
    private readonly SqlConnectionFactory _connectionFactory;
    private readonly PasswordHasher _passwordHasher;
    private readonly JwtTokenFactory _tokenFactory;

    /// <summary>
    /// 透過建構式注入所需依賴，方便 Mock 與測試。
    /// </summary>
    /// <param name="connectionFactory">提供資料庫連線工廠。</param>
    /// <param name="passwordHasher">負責密碼雜湊與驗證的元件。</param>
    /// <param name="tokenFactory">產生與驗證 JWT 的元件。</param>
    public AuthService(
        SqlConnectionFactory connectionFactory,
        PasswordHasher passwordHasher,
        JwtTokenFactory tokenFactory)
    {
        _connectionFactory = connectionFactory;
        _passwordHasher = passwordHasher;
        _tokenFactory = tokenFactory;
    }

    /// <inheritdoc />
    public Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Login flow is not implemented yet.");
    }

    /// <inheritdoc />
    public Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Refresh token flow is not implemented yet.");
    }

    /// <inheritdoc />
    public Task LogoutAsync(LogoutRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Logout flow is not implemented yet.");
    }
}
