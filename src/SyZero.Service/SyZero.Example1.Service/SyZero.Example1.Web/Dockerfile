FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["SyZero.Example1.Service/SyZero.Example1.Web/SyZero.Example1.Web.csproj", "SyZero.Example1.Service/SyZero.Example1.Web/"]
COPY ["SyZero.Example1.Service/SyZero.Example1.Application/SyZero.Example1.Application.csproj", "SyZero.Example1.Service/SyZero.Example1.Application/"]
COPY ["SyZero.Example1.Service/SyZero.Example1.Core/SyZero.Example1.Core.csproj", "SyZero.Example1.Service/SyZero.Example1.Core/"]
COPY ["SyZero.Example1.Service/SyZero.Example1.IApplication/SyZero.Example1.IApplication.csproj", "SyZero.Example1.Service/SyZero.Example1.IApplication/"]
RUN dotnet restore "SyZero.Example1.Service/SyZero.Example1.Web/SyZero.Example1.Web.csproj"
COPY . .
WORKDIR "/src/SyZero.Example1.Service/SyZero.Example1.Web"
RUN dotnet build "SyZero.Example1.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SyZero.Example1.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SyZero.Example1.Web.dll"]
