#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HealthTrackingManageAPI/HealthTrackingManageAPI.csproj", "HealthTrackingManageAPI/"]
COPY ["BusinessObject/BusinessObject.csproj", "BusinessObject/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
COPY ["Repository/Repository.csproj", "Repository/"]
RUN dotnet restore "./HealthTrackingManageAPI/./HealthTrackingManageAPI.csproj"
COPY . .
WORKDIR "/src/HealthTrackingManageAPI"
RUN dotnet build "./HealthTrackingManageAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HealthTrackingManageAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HealthTrackingManageAPI.dll"]