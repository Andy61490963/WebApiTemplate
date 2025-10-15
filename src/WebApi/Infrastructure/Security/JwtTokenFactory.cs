namespace WebApiTemplate.Infrastructure.Security;

using System.Security.Claims;

/// <summary>
/// 集中管理 JWT 產生與驗證邏輯的骨架類別。
/// </summary>
/// <remarks>
/// 之後可整合組態與金鑰管理，並在此實作簽章與驗證流程。
/// </remarks>
public sealed class JwtTokenFactory
{
    /// <summary>
    /// 建立新的存取權杖。
    /// </summary>
    /// <param name="claims">JWT 內含的宣告。</param>
    /// <param name="expires">權杖到期時間。</param>
    /// <returns>簽章後的權杖字串。</returns>
    /// <remarks>
    /// 實作時請選擇安全的簽章演算法並妥善保護金鑰，避免被偽造。
    /// </remarks>
    public string CreateAccessToken(IEnumerable<Claim> claims, DateTimeOffset expires)
    {
        throw new NotImplementedException("JWT creation logic is not implemented yet.");
    }

    /// <summary>
    /// 建立新的更新權杖。
    /// </summary>
    /// <returns>隨機產生的權杖。</returns>
    /// <remarks>
    /// 建議使用密碼學安全的亂數來源，並設定適當長度以降低猜測機率。
    /// </remarks>
    public string CreateRefreshToken()
    {
        throw new NotImplementedException("Refresh token creation is not implemented yet.");
    }

    /// <summary>
    /// 驗證傳入的權杖是否有效。
    /// </summary>
    /// <param name="token">待驗證的 JWT 字串。</param>
    /// <returns>驗證後的 <see cref="ClaimsPrincipal"/>。</returns>
    /// <remarks>
    /// 實作時需處理過期、簽章錯誤與權杖撤銷等情境。
    /// </remarks>
    public ClaimsPrincipal ValidateToken(string token)
    {
        throw new NotImplementedException("JWT validation logic is not implemented yet.");
    }
}
