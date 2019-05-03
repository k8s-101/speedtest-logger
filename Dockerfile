FROM microsoft/dotnet:2.1-sdk AS build-stage
WORKDIR /SpeedTestLogger

COPY /SpeedTestLogger/SpeedTestLogger.csproj ./
RUN dotnet restore

COPY /SpeedTestLogger ./
RUN dotnet publish \
    --output ./PublishedApp \
    --configuration Release \
    --no-restore

FROM microsoft/dotnet:2.1-aspnetcore-runtime
LABEL repository="github.com/k8s-101/speedtest-logger"
WORKDIR /SpeedTestLogger

COPY --from=build-stage /SpeedTestLogger/PublishedApp ./
ENTRYPOINT ["dotnet", "SpeedTestLogger.dll"]