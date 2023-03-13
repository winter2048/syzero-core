set source_url="https://nuget.pkg.github.com/OWNER/index.json"

echo 推送至%source_url%

for /f "delims=" %%a in ('dir /b/a-d/oN %~dp0..\..\..\nuget\*.nupkg') do (
    echo ""
    echo %%a: 开始推送
    dotnet nuget push "%%a" --source "%source_url%"
    echo %%a: 推送完成
)