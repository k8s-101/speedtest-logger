FROM microsoft/dotnet:2.1-sdk AS build-stage
WORKDIR /SpeedTestLogger

COPY /SpeedTestLogger ./
RUN dotnet publish \
    -o /PublishedApp \
    -c Release

FROM microsoft/dotnet:2.1-aspnetcore-runtime
LABEL repository="github.com/k8s-101/speedtest-logger"
WORKDIR /SpeedTestLogger
COPY --from=build-stage /PublishedApp .
ENTRYPOINT ["dotnet", "Detektor.Host.dll"]
