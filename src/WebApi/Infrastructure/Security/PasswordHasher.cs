namespace WebApiTemplate.Infrastructure.Security;

/// <summary>
/// 提供密碼雜湊與驗證功能的骨架類別。
/// </summary>
/// <remarks>
/// 後續可整合 PBKDF2、bcrypt 或 Argon2 等演算法，提升安全性。
/// </remarks>
public sealed class PasswordHasher
{
    /// <summary>
    /// 將明文密碼雜湊為安全字串。
    /// </summary>
    /// <param name="password">使用者輸入的明文密碼。</param>
    /// <returns>雜湊後的密碼。</returns>
    /// <remarks>
    /// 建議使用具備鹽值與反彩虹表攻擊能力的演算法。
    /// </remarks>
    public string HashPassword(string password)
    {
        throw new NotImplementedException("Password hashing is not implemented yet.");
    }

    /// <summary>
    /// 驗證明文密碼與雜湊結果是否一致。
    /// </summary>
    /// <param name="hashedPassword">已儲存的雜湊值。</param>
    /// <param name="password">使用者提供的明文密碼。</param>
    /// <returns>驗證結果。</returns>
    /// <remarks>
    /// 實作時應使用固定時間比較以避免時序攻擊。
    /// </remarks>
    public bool VerifyHashedPassword(string hashedPassword, string password)
    {
        throw new NotImplementedException("Password verification is not implemented yet.");
    }
}
