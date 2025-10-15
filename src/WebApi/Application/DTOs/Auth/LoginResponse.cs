namespace WebApiTemplate.Application.DTOs.Auth;

/// <summary>
/// 提供登入成功後回傳給前端的權杖資訊。
/// </summary>
/// <remarks>
/// 以不可變物件呈現，確保建立後不會被修改，避免權杖被覆寫。
/// </remarks>
public sealed class LoginResponse
{
    /// <summary>
    /// JWT 存取權杖。
    /// </summary>
    public required string AccessToken { get; init; }

    /// <summary>
    /// 更新權杖，供存取權杖過期後換取新權杖。
    /// </summary>
    public required string RefreshToken { get; init; }

    /// <summary>
    /// 存取權杖的有效秒數。
    /// </summary>
    public required int ExpiresIn { get; init; }
}
