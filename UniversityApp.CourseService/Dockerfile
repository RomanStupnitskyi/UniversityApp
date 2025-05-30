FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UniversityApp.CourseService/UniversityApp.CourseService.csproj", "UniversityApp.CourseService/"]
COPY ["UniversityApp.Shared/UniversityApp.Shared.csproj", "UniversityApp.Shared/"]
RUN dotnet restore "UniversityApp.CourseService/UniversityApp.CourseService.csproj"
COPY . .
WORKDIR "/src/UniversityApp.CourseService"
RUN dotnet build "./UniversityApp.CourseService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UniversityApp.CourseService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniversityApp.CourseService.dll"]
