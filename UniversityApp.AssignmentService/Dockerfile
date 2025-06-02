FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8082
EXPOSE 18082

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UniversityApp.AssignmentService/UniversityApp.AssignmentService.csproj", "UniversityApp.AssignmentService/"]
COPY ["UniversityApp.Shared/UniversityApp.Shared.csproj", "UniversityApp.Shared/"]
RUN dotnet restore "UniversityApp.AssignmentService/UniversityApp.AssignmentService.csproj"
COPY . .
WORKDIR "/src/UniversityApp.AssignmentService"
RUN dotnet build "./UniversityApp.AssignmentService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UniversityApp.AssignmentService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniversityApp.AssignmentService.dll"]
