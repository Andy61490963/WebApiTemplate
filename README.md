# WebApiTemplate

此專案提供登入流程的骨架，採用分層架構將 API、應用層與基礎設施層分離，以方便後續擴充與測試。

## 架構說明

```
src/WebApi
├─ Program.cs                     // 註冊服務與建置中介軟體管線
├─ appsettings.json               // 組態，包含資料庫連線字串
├─ Controllers
│  └─ LoginController.cs          // 提供登入、刷新與登出端點
├─ Filters
│  └─ ApiExceptionFilter.cs       // 統一處理例外並回傳 ProblemDetails
├─ Application
│  ├─ Interfaces
│  │  └─ IAuthService.cs          // 登入相關服務介面
│  └─ DTOs
│     └─ Auth
│        ├─ LoginRequest.cs       // 登入請求模型
│        ├─ LoginResponse.cs      // 登入回應模型
│        ├─ RefreshTokenRequest.cs// 更新權杖請求模型
│        └─ LogoutRequest.cs      // 登出請求模型
└─ Infrastructure
   ├─ Db
   │  ├─ DatabaseOptions.cs       // 資料庫組態封裝
   │  └─ SqlConnectionFactory.cs  // 產生 SQL 連線，後續供 Dapper 使用
   ├─ Security
   │  ├─ JwtTokenFactory.cs       // JWT 產生與驗證骨架
   │  └─ PasswordHasher.cs        // 密碼雜湊骨架
   └─ Services
      └─ AuthService.cs           // IAuthService 的預留實作
```

## 呼叫流程

1. 呼叫者向 `LoginController` 發送 HTTP 請求，並由 DTO 的 DataAnnotations 執行輸入驗證。
2. 若模型驗證通過，控制器會呼叫 `IAuthService`，將責任交給基礎設施層的 `AuthService`。
3. `AuthService` 未來會透過 `SqlConnectionFactory` 建立連線並使用 Dapper 操作資料庫，同時整合 `PasswordHasher` 與 `JwtTokenFactory`。
4. 若流程發生例外，`ApiExceptionFilter` 會攔截並回傳標準 `ProblemDetails`。

## 後續實作建議

- 在 `AuthService` 中使用 Dapper 撰寫查詢，並確保使用參數化避免 SQL Injection。
- 於 `JwtTokenFactory` 加入金鑰管理與權杖簽章邏輯，建議將敏感資訊放置於安全存放機制。
- 在 `PasswordHasher` 實作安全演算法（例如 PBKDF2 或 bcrypt），並採固定時間比較避免時序攻擊。
- 撰寫單元測試驗證各層邏輯，並於 CI 中自動執行。
