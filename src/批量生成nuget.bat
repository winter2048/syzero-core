@echo off
set nowPath=%cd%
cd \
cd %nowPath%
echo 开始生成

echo 开始生成SyZero
cd SyZero.Core\SyZero
del /f /s /q %USERPROFILE%\.nuget\packages\SyZero
del /f /s /q bin\Debug\*.nupkg
dotnet pack
copy /y bin\Debug\*.nupkg ..\..\..\nuget\*.nupkg
cd %nowPath%


echo 开始生成SyZero.ApiGateway
cd SyZero.Core\SyZero.ApiGateway
del /f /s /q %USERPROFILE%\.nuget\packages\SyZero.ApiGateway
del /f /s /q bin\Debug\*.nupkg
dotnet pack
copy /y bin\Debug\*.nupkg ..\..\..\nuget\*.nupkg
cd %nowPath%

echo 开始生成SyZero.AspNetCore
cd SyZero.Core\SyZero.AspNetCore
del /f /s /q %USERPROFILE%\.nuget\packages\SyZero.AspNetCore
del /f /s /q bin\Debug\*.nupkg
dotnet pack
copy /y bin\Debug\*.nupkg ..\..\..\nuget\*.nupkg
cd %nowPath%

echo 开始生成SyZero.AutoMapper
cd SyZero.Core\SyZero.AutoMapper
del /f /s /q %USERPROFILE%\.nuget\packages\SyZero.AutoMapper
del /f /s /q bin\Debug\*.nupkg
dotnet pack
copy /y bin\Debug\*.nupkg ..\..\..\nuget\*.nupkg
cd %nowPath%

echo 开始生成SyZero.Consul
cd SyZero.Core\SyZero.Consul
del /f /s /q %USERPROFILE%\.nuget\packages\SyZero.Consul
del /f /s /q bin\Debug\*.nupkg
dotnet pack
copy /y bin\Debug\*.nupkg ..\..\..\nuget\*.nupkg
cd %nowPath%

echo 开始生成SyZero.DynamicWebApi
cd SyZero.Core\SyZero.DynamicWebApi
del /f /s /q %USERPROFILE%\.nuget\packages\SyZero.DynamicWebApi
del /f /s /q bin\Debug\*.nupkg
dotnet pack
copy /y bin\Debug\*.nupkg ..\..\..\nuget\*.nupkg
cd %nowPath%

echo 开始生成SyZero.EntityFrameworkCore
cd SyZero.Core\SyZero.EntityFrameworkCore
del /f /s /q %USERPROFILE%\.nuget\packages\SyZero.EntityFrameworkCore
del /f /s /q bin\Debug\*.nupkg
dotnet pack
copy /y bin\Debug\*.nupkg ..\..\..\nuget\*.nupkg
cd %nowPath%

echo 开始生成SyZero.Feign
cd SyZero.Core\SyZero.Feign
del /f /s /q %USERPROFILE%\.nuget\packages\SyZero.Feign
del /f /s /q bin\Debug\*.nupkg
dotnet pack
copy /y bin\Debug\*.nupkg ..\..\..\nuget\*.nupkg
cd %nowPath%

echo 开始生成SyZero.Log4Net
cd SyZero.Core\SyZero.Log4Net
del /f /s /q %USERPROFILE%\.nuget\packages\SyZero.Log4Net
del /f /s /q bin\Debug\*.nupkg
dotnet pack
copy /y bin\Debug\*.nupkg ..\..\..\nuget\*.nupkg
cd %nowPath%

echo 开始生成SyZero.MongoDB
cd SyZero.Core\SyZero.MongoDB
del /f /s /q %USERPROFILE%\.nuget\packages\SyZero.MongoDB
del /f /s /q bin\Debug\*.nupkg
dotnet pack
copy /y bin\Debug\*.nupkg ..\..\..\nuget\*.nupkg
cd %nowPath%

echo 开始生成SyZero.Nacos
cd SyZero.Core\SyZero.Nacos
del /f /s /q %USERPROFILE%\.nuget\packages\SyZero.Nacos
del /f /s /q bin\Debug\*.nupkg
dotnet pack
copy /y bin\Debug\*.nupkg ..\..\..\nuget\*.nupkg
cd %nowPath%

echo 开始生成SyZero.RabbitMQ
cd SyZero.Core\SyZero.RabbitMQ
del /f /s /q %USERPROFILE%\.nuget\packages\SyZero.RabbitMQ
del /f /s /q bin\Debug\*.nupkg
dotnet pack
copy /y bin\Debug\*.nupkg ..\..\..\nuget\*.nupkg
cd %nowPath%

echo 开始生成SyZero.Redis
cd SyZero.Core\SyZero.Redis
del /f /s /q %USERPROFILE%\.nuget\packages\SyZero.Redis
del /f /s /q bin\Debug\*.nupkg
dotnet pack
copy /y bin\Debug\*.nupkg ..\..\..\nuget\*.nupkg
cd %nowPath%

echo 开始生成SyZero.SqlSugar
cd SyZero.Core\SyZero.SqlSugar
del /f /s /q %USERPROFILE%\.nuget\packages\SyZero.SqlSugar
del /f /s /q bin\Debug\*.nupkg
dotnet pack
copy /y bin\Debug\*.nupkg ..\..\..\nuget\*.nupkg
cd %nowPath%

echo 开始生成SyZero.Swagger
cd SyZero.Core\SyZero.Swagger
del /f /s /q %USERPROFILE%\.nuget\packages\SyZero.Swagger
del /f /s /q bin\Debug\*.nupkg
dotnet pack
copy /y bin\Debug\*.nupkg ..\..\..\nuget\*.nupkg
cd %nowPath%

echo 开始生成SyZero.Web.Common
cd SyZero.Core\SyZero.Web.Common
del /f /s /q %USERPROFILE%\.nuget\packages\SyZero.Web.Common
del /f /s /q bin\Debug\*.nupkg
dotnet pack
copy /y bin\Debug\*.nupkg ..\..\..\nuget\*.nupkg
cd %nowPath%

echo 开始生成SyZero.Gateway
cd SyZero.Core\SyZero.Gateway
del /f /s /q %USERPROFILE%\.nuget\packages\SyZero.Gateway
del /f /s /q bin\Debug\*.nupkg
dotnet pack
copy /y bin\Debug\*.nupkg ..\..\..\nuget\*.nupkg
cd %nowPath%

echo 完成
Pause