FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base

WORKDIR /app
COPY ..
EXPOSE 80  


ENTRYPOINT ["dotnet", "SyZeroBlog.Web.dll"]