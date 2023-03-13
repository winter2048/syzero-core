@echo off
set nowPath=%cd%
cd \
cd %nowPath%
echo 开始生成

dotnet build SyZero.sln --configuration Release
dotnet pack SyZero.sln --configuration Release

echo 完成
Pause