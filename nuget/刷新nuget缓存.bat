@echo off
set nowPath=%cd%
cd \
cd %nowPath%
echo 开始生成

del /f /s /q %USERPROFILE%\.nuget\packages\SyZero

del /f /s /q %USERPROFILE%\.nuget\packages\SyZero.ApiGateway

del /f /s /q %USERPROFILE%\.nuget\packages\SyZero.AspNetCore

del /f /s /q %USERPROFILE%\.nuget\packages\SyZero.AutoMapper

del /f /s /q %USERPROFILE%\.nuget\packages\SyZero.Consul

del /f /s /q %USERPROFILE%\.nuget\packages\SyZero.EntityFrameworkCore

del /f /s /q %USERPROFILE%\.nuget\packages\SyZero.Log4Net

del /f /s /q %USERPROFILE%\.nuget\packages\SyZero.MongoDB

del /f /s /q %USERPROFILE%\.nuget\packages\SyZero.Nacos

del /f /s /q %USERPROFILE%\.nuget\packages\SyZero.Redis

del /f /s /q %USERPROFILE%\.nuget\packages\SyZero.Web.Common

echo 完成
Pause