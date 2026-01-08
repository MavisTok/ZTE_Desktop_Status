# ZTE Router Dashboard

一个用于监控 ZTE U60Pro的 WPF 桌面应用程序。

## 功能特性

### 实时监控面板

1. **网络状态**
   - 连接状态 (IPv4/IPv6)
   - IP 地址显示
   - DNS 服务器信息

2. **实时速率**
   - 上传速度 (实时显示)
   - 下载速度 (实时显示)
   - 自动单位转换 (B/s, KB/s, MB/s, GB/s)

3. **流量统计**
   - 今日流量
   - 本月流量
   - 总流量

4. **SIM 卡信息**
   - 运营商名称
   - SIM 卡状态
   - ICCID (脱敏显示)
   - MAC 地址

5. **信号强度**
   - 网络类型 (5G SA/NSA, 4G LTE)
   - 频段信息
   - 信号格数 (0-5)
   - RSRP/RSRQ/SNR 详细信号参数

6. **设备统计**
   - 在线设备总数
   - 无线连接设备
   - 有线连接设备

## 技术架构

### 项目结构

```
ZTE/
├── Api/                      # API 通信层
│   ├── UbusClient.cs        # Ubus JSON-RPC 客户端
│   └── SessionManager.cs    # Session 生命周期管理
├── Models/                   # 数据模型
│   ├── SpeedStat.cs         # 速率和流量统计
│   ├── SimInfo.cs           # SIM 卡信息
│   ├── NetInfo.cs           # 网络和信号信息
│   ├── DeviceCount.cs       # 设备数量统计
│   ├── WifiReport.cs        # WiFi 配置信息
│   └── WanStatus.cs         # WAN 接口状态
├── Services/                 # 业务服务层
│   └── HomeDashboardService.cs  # 仪表盘数据服务
├── ViewModels/              # MVVM 视图模型
│   ├── ViewModelBase.cs     # ViewModel 基类
│   └── DashboardViewModel.cs # 仪表盘 ViewModel
├── Utils/                    # 工具类
│   └── FormatUtil.cs        # 格式化工具 (速率/流量/脱敏)
└── MainWindow.xaml          # 主界面
```

### 技术栈

- **框架**: .NET Framework 4.7.2
- **UI**: WPF (Windows Presentation Foundation)
- **架构模式**: MVVM (Model-View-ViewModel)
- **HTTP 客户端**: System.Net.Http
- **JSON 序列化**: System.Text.Json
- **数据绑定**: INotifyPropertyChanged

### 核心设计

#### 1. Ubus JSON-RPC 通信

应用程序直接调用路由器的 ubus 接口,不依赖网页解析:

```csharp
// 批量请求多个接口
var calls = new List<object>
{
    client.BuildCall("zwrt_data", "get_wwandst", new {}, 46),    // 速率统计
    client.BuildCall("zwrt_data", "getbasicinfo", new {}, 47),   // SIM 信息
    client.BuildCall("zwrt_data", "get_nrinfo", new {}, 48),     // 信号信息
    // ...
};

var results = await client.BatchCallAsync(calls);
```

#### 2. 自动刷新机制

- 默认每 2 秒刷新一次数据
- 使用 `DispatcherTimer` 确保 UI 线程安全
- 批量请求减少网络开销

#### 3. 数据格式化

```csharp
// 速率格式化: 561796 B/s → "548.7 KB/s"
FormatUtil.FormatSpeed(bytesPerSec)

// 流量格式化: 1073741824 B → "1.00 GB"
FormatUtil.FormatBytes(bytes)

// 敏感信息脱敏: "89860123456789012345" → "8986***2345"
FormatUtil.MaskString(iccid)
```

## 使用说明

### 前置条件

1. Windows 操作系统
2. .NET Framework 4.7.2 或更高版本
3. ZTE 路由器 (支持 ubus 接口)
4. 确保电脑能访问路由器 (通常是 192.168.0.1)

### 配置说明

#### 修改路由器地址

如果你的路由器地址不是 `192.168.0.1`,请修改 `MainWindow.xaml.cs` 文件:

```csharp
private void InitializeApp()
{
    // 修改这里的 URL
    string routerUrl = "http://192.168.1.1";  // 改为你的路由器地址
    // ...
}
```

#### 获取 Session Token

当前版本使用固定的 session token。要获取你的路由器的 session:

1. 打开浏览器,访问路由器管理页面
2. 按 F12 打开开发者工具
3. 切换到 Network 标签
4. 刷新页面
5. 查找 `/ubus/` 请求
6. 在请求 payload 中找到 session 字段的值
7. 将该值更新到代码中:

```csharp
string session = "你的session值";
```

**注意**: Session 可能会过期,需要定期更新。

### 运行应用

1. 打开 Visual Studio
2. 还原 NuGet 包: `nuget restore` 或在 VS 中右键解决方案 → "还原 NuGet 包"
3. 按 F5 运行或点击"开始"按钮

### 停止自动刷新

关闭窗口时,应用会自动停止数据刷新。

## 待完善功能

### 短期改进

- [ ] 实现自动 session 创建/登录 (需要抓取 session 创建接口)
- [ ] 添加连接错误重试机制
- [ ] 支持配置文件 (App.config)
- [ ] 添加日志记录功能

### 长期规划

- [ ] 流量趋势图表 (使用 LiveCharts)
- [ ] 信号强度历史曲线
- [ ] 连接设备列表详情
- [ ] WiFi 密码显示和修改
- [ ] 路由器重启功能
- [ ] 托盘图标和最小化
- [ ] 多语言支持

## API 接口说明

### 已实现的 Ubus 调用

| ID | 对象 | 函数 | 说明 |
|----|------|------|------|
| 46 | zwrt_data | get_wwandst | 速率和流量统计 |
| 47 | zwrt_data | getbasicinfo | SIM 卡基本信息 |
| 48 | zwrt_data | get_nrinfo | 网络类型和信号强度 |
| 51 | zwrt_data | get_stanum | 连接设备数量 |
| 53 | zwrt_wifi | get_reportwifi | WiFi 配置信息 |
| 55 | zwrt_data | get_wwaniface | WAN 接口状态 |

### 数据单位说明

- **速率**: 接口返回 bytes/s,程序自动转换为 KB/s 或 MB/s
- **流量**: 接口返回 bytes,程序自动转换为 KB/MB/GB
- **信号**: RSRP/RSRQ 单位为 dBm 或 dB

## 问题排查

### 应用无法连接路由器

1. 检查路由器 IP 地址是否正确
2. 确认电脑已连接到路由器
3. 尝试在浏览器访问路由器管理页面

### 数据显示为 "N/A" 或 0

1. Session token 可能已过期,需要更新
2. 路由器接口可能返回了错误
3. 检查 `StatusMessage` 显示的错误信息

### 编译错误

1. 确保已还原 NuGet 包
2. 检查 .NET Framework 版本是否为 4.7.2+
3. 清理并重新生成解决方案

## 开发参考

基于设计方案文档: `项目规划\设计方案.md`

## 许可证

本项目仅用于学习和个人使用。
