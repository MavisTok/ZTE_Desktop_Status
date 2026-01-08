@echo off
echo ========================================
echo ZTE Router Dashboard - 安装 NuGet 包
echo ========================================
echo.

echo 正在检查 NuGet...
where nuget >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo [错误] 未找到 nuget 命令
    echo.
    echo 请使用以下方法之一安装 NuGet 包:
    echo.
    echo 方法 1: 使用 Visual Studio
    echo   1. 在 Visual Studio 中打开项目
    echo   2. 右键点击解决方案
    echo   3. 选择 "还原 NuGet 程序包"
    echo.
    echo 方法 2: 使用 dotnet CLI
    echo   1. 打开命令提示符
    echo   2. 运行: dotnet restore "ZTE.csproj"
    echo.
    echo 方法 3: 下载 NuGet.exe
    echo   1. 访问 https://www.nuget.org/downloads
    echo   2. 下载 nuget.exe 到项目目录
    echo   3. 运行: nuget restore
    echo.
    pause
    exit /b 1
)

echo 找到 NuGet,正在还原包...
nuget restore "ZTE.csproj"

if %ERRORLEVEL% EQU 0 (
    echo.
    echo [成功] NuGet 包已成功还原!
    echo.
    echo 现在可以在 Visual Studio 中打开并编译项目了。
) else (
    echo.
    echo [错误] NuGet 包还原失败
    echo 请在 Visual Studio 中手动还原包
)

echo.
pause
