using System;

namespace WebApiTemplate.Infrastructure.Security;

/// <summary>
/// 提供密碼雜湊與驗證功能的工具類別。
/// </summary>
/// <remarks>Remark: 待導入實際雜湊演算法。</remarks>
public sealed class PasswordHasher
{
    /// <summary>
    /// 產生密碼雜湊值。
    /// </summary>
    /// <param name="plainPassword">原始密碼。</param>
    /// <returns>雜湊後字串。</returns>
    public string HashPassword(string plainPassword)
    {
        throw new NotImplementedException("密碼雜湊邏輯尚未實作。");
    }

    /// <summary>
    /// 驗證密碼是否符合雜湊值。
    /// </summary>
    /// <param name="hashedPassword">儲存的雜湊值。</param>
    /// <param name="plainPassword">原始密碼。</param>
    /// <returns>若驗證成功則為 true。</returns>
    public bool VerifyHashedPassword(string hashedPassword, string plainPassword)
    {
        throw new NotImplementedException("密碼驗證邏輯尚未實作。");
    }
}
