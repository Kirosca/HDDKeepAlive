@echo off
chcp 65001 >nul
echo [HDDKeepAlive] 正在查找 Windows C# 编译器 (csc.exe)...

set CSC=""
if exist "%windir%\Microsoft.NET\Framework64\v4.0.30319\csc.exe" (
    set CSC="%windir%\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
) else if exist "%windir%\Microsoft.NET\Framework\v4.0.30319\csc.exe" (
    set CSC="%windir%\Microsoft.NET\Framework\v4.0.30319\csc.exe"
)

if %CSC%=="" (
    echo [错误] 未找到 .NET Framework 4.x 编译器。
    pause
    exit /b 1
)

echo [HDDKeepAlive] 使用编译器: %CSC%
echo [HDDKeepAlive] 正在编译单文件可执行程序 HDDKeepAlive.exe...

%CSC% /target:winexe /optimize+ /win32icon:app.ico /r:System.Drawing.dll,System.Windows.Forms.dll,System.Management.dll /out:HDDKeepAlive.exe Program.cs

if %ERRORLEVEL% EQU 0 (
    echo [成功] 编译完成: HDDKeepAlive.exe
) else (
    echo [失败] 编译出错，返回码: %ERRORLEVEL%
)

pause
