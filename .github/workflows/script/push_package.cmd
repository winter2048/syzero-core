@echo off

set source_url="https://nuget.pkg.github.com/winter2048/index.json"

echo 推送至%source_url%

cd %~dp0..\..\..\nuget
for /f "delims=" %%a in ('dir /b/a-d/oN *.nupkg') do (
    echo ""
    echo %%a: 开始推送
    dotnet nuget push "%%a" --api-key "%GB_NUGET_TOKEN%" --source "%source_url%" --skip-duplicate
    echo %%a: 推送完成
)