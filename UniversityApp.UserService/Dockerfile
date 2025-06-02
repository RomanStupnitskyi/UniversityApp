FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UniversityApp.UserService/UniversityApp.UserService.csproj", "UniversityApp.UserService/"]
COPY ["UniversityApp.Shared/UniversityApp.Shared.csproj", "UniversityApp.Shared/"]
RUN dotnet restore "UniversityApp.UserService/UniversityApp.UserService.csproj"
COPY . .
WORKDIR "/src/UniversityApp.UserService"
RUN dotnet build "./UniversityApp.UserService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UniversityApp.UserService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniversityApp.UserService.dll"]
