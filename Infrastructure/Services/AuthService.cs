using System;
using WebApiTemplate.Application.DTOs.Auth;
using WebApiTemplate.Application.Interfaces;
using WebApiTemplate.Infrastructure.Db;
using WebApiTemplate.Infrastructure.Security;

namespace WebApiTemplate.Infrastructure.Services;

/// <summary>
/// 提供與認證相關的資料處理服務。
/// </summary>
/// <remarks>Remark: 實際邏輯待後續實作，現階段僅保留骨架。</remarks>
public sealed class AuthService : IAuthService
{
    private readonly SqlConnectionFactory _connectionFactory;
    private readonly JwtTokenFactory _jwtTokenFactory;
    private readonly PasswordHasher _passwordHasher;

    /// <summary>
    /// 初始化 <see cref="AuthService"/> 類別的新執行個體。
    /// </summary>
    /// <param name="connectionFactory">資料庫連線工廠。</param>
    /// <param name="jwtTokenFactory">JWT 產生工廠。</param>
    /// <param name="passwordHasher">密碼雜湊工具。</param>
    public AuthService(SqlConnectionFactory connectionFactory, JwtTokenFactory jwtTokenFactory, PasswordHasher passwordHasher)
    {
        _connectionFactory = connectionFactory;
        _jwtTokenFactory = jwtTokenFactory;
        _passwordHasher = passwordHasher;
    }

    /// <inheritdoc />
    public Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("Login 邏輯尚未實作。");
    }

    /// <inheritdoc />
    public Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("Refresh token 邏輯尚未實作。");
    }

    /// <inheritdoc />
    public Task LogoutAsync(LogoutRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("Logout 邏輯尚未實作。");
    }
}
