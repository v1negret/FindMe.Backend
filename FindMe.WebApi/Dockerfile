FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FindMe.WebApi/FindMe.WebApi.csproj", "FindMe.WebApi/"]
COPY ["FindMe.Data/FindMe.Data.csproj", "FindMe.Data/"]
COPY ["FindMe.Models/FindMe.Models.csproj", "FindMe.Models/"]
COPY ["FindMe.Services/FindMe.Services.csproj", "FindMe.Services/"]
RUN dotnet restore "FindMe.WebApi/FindMe.WebApi.csproj"
COPY . .
WORKDIR "/src/FindMe.WebApi"
RUN dotnet build "FindMe.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FindMe.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FindMe.WebApi.dll"]
