# Imagen de MockIntegrations (.NET 10).
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY global.json ./
COPY src/MockIntegrations.Api/MockIntegrations.Api.csproj src/MockIntegrations.Api/
RUN dotnet restore src/MockIntegrations.Api/MockIntegrations.Api.csproj
COPY src/ src/
RUN dotnet publish src/MockIntegrations.Api/MockIntegrations.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 5080 2525
ENTRYPOINT ["dotnet", "MockIntegrations.Api.dll"]
