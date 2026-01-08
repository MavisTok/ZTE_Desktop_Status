# JSON-RPC 数据查看器实现说明

## 概述
已成功创建一个新的 WPF 窗口，用于显示从 HAR 文件（192.168.0.1.har）中提取的所有 JSON-RPC 请求和响应数据。

## 实现的功能

### 1. HAR 文件解析
- **文件**: `Utils/HarParser.cs`
- **功能**: 解析 HAR 文件，提取所有包含 `jsonrpc` 的请求和响应
- **方法**: `ParseJsonRpcData(string harFilePath)` - 返回 JSON-RPC 数据列表

### 2. 数据模型
- **文件**: `Models/JsonRpcData.cs`
- **属性**:
  - `Id`: JSON-RPC 请求 ID
  - `Method`: JSON-RPC 方法名
  - `ApiNamespace`: API 命名空间（如 zwrt_wlan）
  - `ApiFunction`: API 函数名（如 report）
  - `RequestJson`: 完整的请求 JSON
  - `ResponseJson`: 完整的响应 JSON
  - `Timestamp`: 请求时间戳
  - `Url`: 请求 URL
  - `DisplaySummary`: 用于 UI 显示的摘要

### 3. JSON-RPC 查看器窗口
- **文件**: `JsonRpcWindow.xaml` 和 `JsonRpcWindow.xaml.cs`
- **布局**:
  - 左侧面板：显示所有 JSON-RPC 调用的列表
  - 右侧面板：显示选中调用的详细信息（请求和响应）
- **特性**:
  - 自动格式化 JSON 数据
  - 请求用橙色背景显示
  - 响应用绿色背景显示
  - 支持滚动查看

### 4. ViewModel
- **文件**: `ViewModels/JsonRpcViewModel.cs`
- **功能**:
  - 管理 JSON-RPC 数据列表
  - 处理选中项变化
  - 自动格式化 JSON 数据
  - 实现 INotifyPropertyChanged 接口

### 5. 主窗口集成
- 在主窗口头部添加了"📋 JSON-RPC 数据"按钮
- 点击按钮打开 JSON-RPC 查看器窗口
- 按钮带有悬停效果

## 文件清单

### 新增文件
1. `Models/JsonRpcData.cs` - 数据模型
2. `Utils/HarParser.cs` - HAR 文件解析器
3. `ViewModels/JsonRpcViewModel.cs` - 视图模型
4. `JsonRpcWindow.xaml` - 窗口界面
5. `JsonRpcWindow.xaml.cs` - 窗口代码逻辑

### 修改文件
1. `MainWindow.xaml` - 添加了导航按钮
2. `MainWindow.xaml.cs` - 添加了按钮点击事件处理
3. `ZTE.csproj` - 添加了所有新文件的引用

## 数据显示

程序会自动从以下位置查找 HAR 文件：
1. `项目根目录/项目规划/192.168.0.1.har`
2. `bin/Debug/项目规划/192.168.0.1.har`

解析器会提取所有包含 `jsonrpc` 关键字的 HTTP 请求，并显示：
- **左侧列表**: 显示所有 JSON-RPC 调用的摘要（ID、API、时间戳）
- **右侧详情**: 显示选中调用的完整请求和响应 JSON

## 使用方法

1. 在 Visual Studio 中打开项目
2. 编译并运行项目
3. 在主窗口头部点击"📋 JSON-RPC 数据"按钮
4. 在新窗口的左侧列表中选择任意 JSON-RPC 调用
5. 右侧会显示该调用的详细请求和响应信息

## 数据内容

从 HAR 文件中可以提取到的主要 JSON-RPC 调用类型：
1. `zwrt_wlan.report` - WiFi 状态报告
2. `zte_nwinfo_api.nwinfo_get_netinfo` - 网络信息获取
3. `zwrt_zte_mdm.api.get_sim_info_before` - SIM 卡信息
4. `zwrt_mc.device.manager.get_device_info` - 设备信息（电池等）
5. `zwrt_deviceui.zwrt_deviceui_direct_power_mode_get` - 电源模式
6. 其他自定义 API 调用

## 技术特点

- **自动解析**: 无需手动处理，自动从 HAR 文件提取所有数据
- **格式化显示**: JSON 自动缩进格式化，便于阅读
- **双面板布局**: 列表+详情的经典 UI 模式
- **完整信息**: 显示请求 ID、方法、API、时间戳、URL、请求体、响应体
- **错误处理**: 完善的异常处理和用户提示

## 注意事项

1. HAR 文件需要放在正确的位置
2. 需要在 Visual Studio 中编译运行（使用 MSBuild）
3. 支持 .NET Framework 4.7.2
4. 使用 System.Text.Json 进行 JSON 解析
