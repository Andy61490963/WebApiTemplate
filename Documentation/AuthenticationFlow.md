# 認證模組架構說明

本文檔描述目前專案內認證模組的分層與資料流程。程式碼尚未實作商業邏輯，但已完成架構骨幹，方便後續開發。

## 高階流程
1. **控制器層** (`Controllers/LoginController.cs`): 接收外部請求，依據 DTO 進行模型驗證，並呼叫應用服務。
2. **應用層介面** (`Application/Interfaces/IAuthService.cs`): 定義登入、刷新、登出等作業的抽象合約，以保持控制器與實作之間的鬆耦合。
3. **基礎設施層** (`Infrastructure`): 實作資料存取、密碼雜湊、JWT 操作等細節，未來會透過 Dapper 與資料庫互動。
4. **過濾器** (`Filters/ApiExceptionFilter.cs`): 在 MVC 管線中攔截未處理例外，轉換為一致的 `ProblemDetails` 回應。

## 元件關係
```text
Client -> LoginController -> IAuthService -> AuthService
                                |              |-- SqlConnectionFactory (Dapper)
                                |              |-- JwtTokenFactory
                                |              \-- PasswordHasher
```

## DTO 驗證
所有請求 DTO 皆使用 `System.ComponentModel.DataAnnotations` 屬性定義欄位限制。
- 於 `[ApiController]` 控制器內，自動觸發模型驗證。若不通過，框架會回傳 400。
- 成功後才會呼叫 `IAuthService`，符合「驗證通過才進入 Infrastructure」的要求。

## Swagger 與例外處理
- `Program.cs` 內註冊 `AddSwaggerGen` 並於所有環境啟用 `UseSwagger`/`UseSwaggerUI`，確保 API 文件可用。
- 於 `AddControllers` 時註冊 `ApiExceptionFilter`，保證例外時回應統一格式。

## 後續實作建議
- 於 `AuthService` 使用 `SqlConnectionFactory.CreateConnection()` 搭配 Dapper 查詢使用者資料，並與 `PasswordHasher`、`JwtTokenFactory` 整合。
- 在 `JwtTokenFactory` 注入 `IOptions<JwtOptions>` 以設定密鑰與到期時間。
- 擴充 `PasswordHasher` 使用例如 `Microsoft.AspNetCore.Identity.PasswordHasher` 或 `BCrypt` 以提升安全性。

## 安全性與測試
- 實作資料存取時應使用參數化查詢避免 SQL Injection。
- 與 JWT、刷新令牌相關的流程需注意令牌儲存及失效策略。
- 完成實作後，建議撰寫單元測試驗證各元件邏輯，並建立整合測試覆蓋主要 API 流程。
