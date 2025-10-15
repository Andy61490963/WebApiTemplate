using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace WebApiTemplate.Infrastructure.Security;

/// <summary>
/// 負責產生與驗證 JWT 的工廠類別。
/// </summary>
/// <remarks>Remark: 目前僅定義介面，待後續加入實作細節。</remarks>
public sealed class JwtTokenFactory
{
    /// <summary>
    /// 產生新的 JWT 存取令牌。
    /// </summary>
    /// <param name="claims">包含於令牌中的宣告。</param>
    /// <returns>JWT 字串。</returns>
    public string CreateAccessToken(IEnumerable<Claim> claims)
    {
        throw new NotImplementedException("JWT 產生邏輯尚未實作。");
    }

    /// <summary>
    /// 驗證 JWT 是否有效。
    /// </summary>
    /// <param name="token">待驗證的 JWT 字串。</param>
    /// <returns>若驗證成功則回傳宣告集合。</returns>
    public ClaimsPrincipal ValidateToken(string token)
    {
        throw new NotImplementedException("JWT 驗證邏輯尚未實作。");
    }
}
