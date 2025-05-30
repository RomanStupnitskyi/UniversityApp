FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 7000
EXPOSE 7001

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UniversityApp.ApiGateway/UniversityApp.ApiGateway.csproj", "UniversityApp.ApiGateway/"]
RUN dotnet restore "UniversityApp.ApiGateway/UniversityApp.ApiGateway.csproj"
COPY . .
WORKDIR "/src/UniversityApp.ApiGateway"
RUN dotnet build "./UniversityApp.ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UniversityApp.ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniversityApp.ApiGateway.dll"]
