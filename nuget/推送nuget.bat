@echo off
set nowPath=%cd%
cd \
cd %nowPath%
echo.请输入你要推送的目标，比如：github/ms
set /p source=
echo.请输入你的api_key
set /p api_key=
if "%source%" == "github" set source_url="https://nuget.pkg.github.com/OWNER/index.json"
if "%source%" == "ms" set source_url="https://api.nuget.org/v3/index.json"

echo 推送至%source%

for /f "delims=" %%a in ('dir /b/a-d/oN *nupkg*') do (
    echo ""
    echo %%a: 开始推送
    dotnet nuget push "%%a"  --api-key "%api_key%" --source "%source_url%"
    echo %%a: 推送完成
)

echo 完成
Pause