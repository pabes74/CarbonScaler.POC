FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
WORKDIR /app
EXPOSE 8080
COPY bin/Debug/net8.0 .
ENV ASPNETCORE_ENVIRONMENT Development
ENTRYPOINT ["dotnet", "CarbonScaler.CarbonDataAPI.dll"]