# LOADER 排程程式

## 系統概觀
- **框架**：.NET 8 BackgroundService
- **排程方式**：`PeriodicTimer` 以秒為單位觸發
- **記錄系統**：Serilog（檔案 + Windows EventLog）
- **資料存取**：Dapper + SQL Server

## 目錄結構
```
LOADER/
├─ Program.cs                     # 主機設定與進入點
├─ appsettings.json               # 排程、連線與記錄設定
├─ Configuration/
│  ├─ SchedulerOptions.cs         # 排程間隔設定
│  ├─ DatabaseOptions.cs          # 資料庫連線設定
│  └─ LoggingOptions.cs           # Serilog 行為設定
├─ DataModels/
│  └─ ExampleDto.cs               # 資料表對應 DTO
├─ Logging/
│  └─ SerilogConfigurator.cs      # Serilog 建構與路徑判斷
├─ Services/
│  ├─ Infrastructure/
│  │  ├─ ISqlConnectionFactory.cs # 建立 DbConnection
│  │  └─ SqlConnectionFactory.cs  # SQL Server 實作
│  ├─ Interface/
│  │  ├─ IDapperRepository.cs     # Dapper 操作介面
│  │  ├─ IDomainEvent.cs          # 領域事件介面
│  │  └─ IJob.cs                  # 排程作業介面
│  ├─ Processing/
│  │  └─ Worker.cs                # BackgroundService 主體
│  └─ Service/
│     ├─ DapperRepository.cs      # 共用 Dapper 邏輯
│     ├─ DomainEvent.cs           # 領域事件基底
│     ├─ ExampleJob.cs            # 範例作業
│     └─ Job.cs                   # Job 抽象基底
└─ Logs/                          # Serilog 產生記錄檔
```

## 執行流程
1. `Program.Main` 建立 `HostApplicationBuilder`，註冊組態與 Serilog。
2. 啟用 Options Pattern 讀取 `appsettings.json`，並驗證排程間隔與連線字串。
3. DI 容器建立 `SqlConnectionFactory`、`DapperRepository` 與 `ExampleJob`，並註冊 `Worker`。
4. `Worker` 啟動後立即執行一次 `ExampleJob`，其後依 `IntervalSeconds` 使用 `PeriodicTimer` 輪詢。
5. `ExampleJob` 透過 Dapper 查詢最新資料，組成 `DomainEvent`，由基底 `Job` 統一記錄事件內容。
6. Serilog 會依環境決定記錄檔目錄，必要時寫入 Windows EventLog。

## 重要設計
- **解耦與可測試性**：接口（`ISqlConnectionFactory`、`IDapperRepository`、`IJob`）隔離具體實作。
- **錯誤處理**：
  - `Job` 基底類別集中例外處理與日誌。
  - `SqlConnectionFactory` 與 `DapperRepository` 針對連線/查詢錯誤寫入日誌後再拋出。
- **設定集中管理**：`SchedulerOptions`、`DatabaseOptions` 與 `LoggingOptions` 將硬編碼移至 `appsettings.json`。
- **安全性**：Dapper 使用參數化查詢，避免 SQL Injection。
- **擴充性**：若需新增 Job，可繼承 `Job` 並註冊為 `IJob`，或建立事件處理流程。

## 佈署建議
- Windows Service：可搭配 `sc create` 或 `PowerShell New-Service`，`<OutputType>WinExe</OutputType>` 已預備無主控台模式。
- Linux systemd：可建立 service 檔案呼叫 `dotnet WebApiTemplate.dll`，同樣常駐。

## 測試建議
- 單元測試：替換 `IDapperRepository` 為記憶體實作或 Mock，驗證 `ExampleJob` 行為。
- 整合測試：使用測試資料庫，確認 Dapper 查詢與事件紀錄輸出正確。
