namespace WebApiTemplate.Application.DTOs.Auth;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// 用於刷新存取權杖的請求模型。
/// </summary>
/// <remarks>
/// 實作權杖輪替策略時，需確保請求資料在進入服務前已被驗證。
/// </remarks>
public sealed class RefreshTokenRequest
{
    /// <summary>
    /// 目前的存取權杖，用於驗證使用者上下文。
    /// </summary>
    [Required]
    public string AccessToken { get; init; } = string.Empty;

    /// <summary>
    /// 對應的更新權杖，用於換取新的存取權杖。
    /// </summary>
    [Required]
    public string RefreshToken { get; init; } = string.Empty;
}
