FROM microsoft/dotnet:2.0-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.0-sdk AS build
WORKDIR /src
COPY TelemetryApp.sln ./
COPY Telemetry.App/Telemetry.App.csproj Telemetry.App/
COPY Telemetry.Domain/Telemetry.Domain.csproj Telemetry.Domain/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/Telemetry.App
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Telemetry.App.dll"]
