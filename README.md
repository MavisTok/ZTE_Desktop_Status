# ZTE Router Dashboard

一个用于监控 ZTE 路由器（如 U60Pro）的 WPF 桌面应用，基于 ubus JSON-RPC 实时展示网络与设备状态。

## 亮点与更新

- 免登录模式：默认 Session Token 为 `00000000000000000000000000000000`，开箱即用。
- 实时仪表盘：网络状态、上下行速率、流量统计、SIM 信息、信号强度等数据实时刷新。
- 扩展信息窗口：WiFi、电池、电源模式、5G/LTE 小区详情、设备硬件等完整信息卡片。
- 信号强度条：RSRP/RSRQ/SNR 以水平进度条展示，绿到红显示强弱状态。
- JSON-RPC 数据查看器：从 HAR 文件解析并查看请求/响应。
- UI 优化：状态信息移至底部状态栏、最小窗口尺寸限制、IPv6 自动换行。

## 快速开始

1. 用 Visual Studio 打开 `ZTE.csproj`。
2. 还原 NuGet 包。
3. 按 `F5` 运行。
4. 如路由器地址不是 `192.168.0.1`，修改 `AppSettings.cs` 中的 `RouterUrl`。

## 免登录与登录模式

- 免登录模式默认开启：无需手动获取 Session Token，适合基础监控。
- 高级功能（如扩展信息窗口中的部分接口）需要真实 Session Token。

修改位置：`AppSettings.cs`

```csharp
public static string RouterUrl { get; set; } = "http://192.168.0.1";
public static string SessionToken { get; set; } = "00000000000000000000000000000000";
```

获取真实 Session 的方式请参考 `免登录模式说明.md` 和 `项目规划/登录分析.md`。

## 功能概览

### 基础仪表盘（主窗口）

- 网络状态（IPv4/IPv6、DNS、连接状态）
- 实时速率（上/下行）
- 流量统计（今日/本月/总计）
- SIM 信息（运营商、状态、ICCID 脱敏）
- 信号强度（5G/4G、RSRP/RSRQ/SNR）

### 扩展信息窗口

- WiFi 详细配置（2.4G/5G SSID、认证模式、射频状态、LBD）
- 电池与电源（电量、温度、充电、模式）
- 5G/LTE 小区与信号参数（PCI、频段、带宽、ARFCN/Cell ID）
- 信号强度条（RSRP/RSRQ/SNR 以进度条显示强弱）
- 设备硬件信息（CPU、内存、存储、温度、IMEI、MAC）

### JSON-RPC 数据查看器

- 解析 `项目规划/192.168.0.1.har` 并展示请求/响应详情
- 双面板列表与详情视图，自动格式化 JSON

## API 调用概览

### 免登录模式（无需认证）

- `zwrt_router.api / router_get_status_no_auth` 路由器状态
- `zwrt_zte_mdm.api / get_sim_info_before` SIM 信息
- `zte_nwinfo_api / nwinfo_get_netinfo` 网络信号信息
- `zwrt_data / get_wwandst` 速率与流量统计
- `zwrt_data / get_wwaniface` WAN 接口状态

### 登录模式（需要真实 Session）

- `zwrt_data / get_stanum` 连接设备统计
- `zwrt_wifi / get_reportwifi` WiFi 配置
- 扩展信息窗口相关 API（电池、电源、硬件、WiFi 详情等）

## 编译与依赖

- 编译步骤与常见问题请参考：`编译指南.md`
- 编译检查清单请参考：`编译检查清单.md`

## 错误排查

- 查看错误排查与诊断方法：`错误诊断指南.md`

## 文档索引

- [更新说明](更新说明.md)
- [免登录模式说明](免登录模式说明.md)
- [UI优化说明](UI优化说明.md)
- [编译指南](编译指南.md)
- [编译检查清单](编译检查清单.md)
- [错误诊断指南](错误诊断指南.md)
- [设计方案](项目规划/设计方案.md)
- [登录分析](项目规划/登录分析.md)
- [JSON-RPC数据查看器实现说明](项目规划/JSON-RPC数据查看器实现说明.md)
- [扩展信息功能实现说明](项目规划/扩展信息功能实现说明.md)
- [完整功能实现说明-含5G小区和设备信息](项目规划/完整功能实现说明-含5G小区和设备信息.md)
- [最终总结-完整的5G小区和设备信息显示](项目规划/最终总结-完整的5G小区和设备信息显示.md)
- [项目实现总结](项目规划/项目实现总结.md)

## 许可证

本项目仅用于学习和个人使用。
