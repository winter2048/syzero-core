@echo off

echo 推送至%2

for /f "delims=" %%a in ('dir /b/a-d/oN %~dp0..\..\..\nuget\*.nupkg') do (
    echo ""
    echo %%a: 开始推送
    dotnet nuget push "%~dp0..\..\..\nuget\%%a" --api-key "%1" --source "%2" --skip-duplicate
    echo %%a: 推送完成
)