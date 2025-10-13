## See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
## This stage is used when running from VS in fast mode (Default for Debug configuration)
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081
#
#
## This stage is used to build the service project
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["src/Api/FileGateway.Api/FileGateway.Api.csproj", "src/Api/FileGateway.Api/"]
#COPY ["src/Application/FileGateway.Application/FileGateway.Application.csproj", "src/Application/FileGateway.Application/"]
#COPY ["src/Domain/FileGateway.Domain/FileGateway.Domain.csproj", "src/Domain/FileGateway.Domain/"]
#COPY ["src/Infrastructure/FileGateway.Infrastructure/FileGateway.Infrastructure.csproj", "src/Infrastructure/FileGateway.Infrastructure/"]
#RUN dotnet restore "./src/Api/FileGateway.Api/FileGateway.Api.csproj"
#COPY . .
#WORKDIR "/src/src/Api/FileGateway.Api"
#RUN dotnet build "./FileGateway.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
## This stage is used to publish the service project to be copied to the final stage
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "./FileGateway.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
## This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "FileGateway.Api.dll"]



# ===========================
# 1️⃣ Base runtime image
# ===========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# ===========================
# 2️⃣ Build stage
# ===========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src


COPY ["Api/FileGateway.Api/FileGateway.Api.csproj", "Api/FileGateway.Api/"]
COPY ["Application/FileGateway.Application/FileGateway.Application.csproj", "Application/FileGateway.Application/"]
COPY ["Domain/FileGateway.Domain/FileGateway.Domain.csproj", "Domain/FileGateway.Domain/"]
COPY ["Infrastructure/FileGateway.Infrastructure/FileGateway.Infrastructure.csproj", "Infrastructure/FileGateway.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "Api/FileGateway.Api/FileGateway.Api.csproj"

COPY . .

# Build project
WORKDIR "/src/Api/FileGateway.Api"
RUN dotnet build "FileGateway.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ===========================
# 3️⃣ Publish stage
# ===========================
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FileGateway.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ===========================
# 4️⃣ Final runtime image
# ===========================
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileGateway.Api.dll"]
