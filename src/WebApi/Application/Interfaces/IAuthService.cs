namespace WebApiTemplate.Application.Interfaces;

using WebApiTemplate.Application.DTOs.Auth;

/// <summary>
/// 提供驗證相關操作的抽象層，確保控制器與具體實作之間保持鬆耦合。
/// </summary>
/// <remarks>
/// 介面方法僅定義流程契約，細節由基礎設施層負責，符合 Clean Architecture 原則。
/// </remarks>
public interface IAuthService
{
    /// <summary>
    /// 驗證使用者登入資訊並產生必要的存取權杖。
    /// </summary>
    /// <param name="request">包含使用者帳號密碼的登入請求。</param>
    /// <param name="cancellationToken">提供取消非同步作業的能力。</param>
    /// <returns>登入後的權杖資訊。</returns>
    /// <remarks>
    /// 實作時需對密碼雜湊與帳號鎖定策略進行保護，避免暴力破解。
    /// </remarks>
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// 使用有效的更新權杖產生新的存取權杖。
    /// </summary>
    /// <param name="request">包含更新權杖的請求。</param>
    /// <param name="cancellationToken">提供取消非同步作業的能力。</param>
    /// <returns>新的權杖資訊。</returns>
    /// <remarks>
    /// 實作需驗證更新權杖是否過期、是否已被撤銷，以防止權杖重放攻擊。
    /// </remarks>
    Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// 將指定的更新權杖標示為無效，以便使用者安全登出。
    /// </summary>
    /// <param name="request">包含要撤銷的權杖資訊。</param>
    /// <param name="cancellationToken">提供取消非同步作業的能力。</param>
    /// <returns>非同步作業。</returns>
    /// <remarks>
    /// 實作需確保撤銷動作具備冪等性，避免重複請求導致錯誤。
    /// </remarks>
    Task LogoutAsync(LogoutRequest request, CancellationToken cancellationToken = default);
}
