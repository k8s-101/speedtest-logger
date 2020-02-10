FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-stage
WORKDIR /SpeedTestLogger

COPY /SpeedTestLogger/SpeedTestLogger.csproj ./
RUN dotnet restore

COPY /SpeedTestLogger ./
RUN dotnet publish \
    --output ./PublishedApp \
    --configuration Release \
    --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
LABEL repository="github.com/k8s-101/speedtest-logger"
WORKDIR /SpeedTestLogger

COPY --from=build-stage /SpeedTestLogger/PublishedApp ./
ENTRYPOINT ["dotnet", "SpeedTestLogger.dll"]