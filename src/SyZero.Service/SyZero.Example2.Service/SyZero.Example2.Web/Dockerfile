FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["SyZero.Example2.Service/SyZero.Example2.Web/SyZero.Example2.Web.csproj", "SyZero.Example2.Service/SyZero.Example2.Web/"]
COPY ["SyZero.Example2.Service/SyZero.Example2.Application/SyZero.Example2.Application.csproj", "SyZero.Example2.Service/SyZero.Example2.Application/"]
COPY ["SyZero.Example2.Service/SyZero.Example2.Core/SyZero.Example2.Core.csproj", "SyZero.Example2.Service/SyZero.Example2.Core/"]
COPY ["SyZero.Example2.Service/SyZero.Example2.IApplication/SyZero.Example2.IApplication.csproj", "SyZero.Example2.Service/SyZero.Example2.IApplication/"]
RUN dotnet restore "SyZero.Example2.Service/SyZero.Example2.Web/SyZero.Example2.Web.csproj"
COPY . .
WORKDIR "/src/SyZero.Example2.Service/SyZero.Example2.Web"
RUN dotnet build "SyZero.Example2.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SyZero.Example2.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SyZero.Example2.Web.dll"]
