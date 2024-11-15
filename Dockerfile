# syntax=docker/dockerfile:1

# Stage 1: Build the application
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

# Set up working directory
WORKDIR /source

# Copy project files
COPY ["dhbw.WebEngineering.V2.Api/dhbw.WebEngineering.V2.Api.csproj", "dhbw.WebEngineering.V2.Api/"]
COPY ["dhbw.WebEngineering.V2.Adapters/dhbw.WebEngineering.V2.Adapters.csproj", "dhbw.WebEngineering.V2.Adapters/"]
COPY ["dhbw.WebEngineering.V2.Application/dhbw.WebEngineering.V2.Application.csproj", "dhbw.WebEngineering.V2.Application/"]
COPY ["dhbw.WebEngineering.V2.Domain/dhbw.WebEngineering.V2.Domain.csproj", "dhbw.WebEngineering.V2.Domain/"]
COPY ["dhbw.WebEngineering.V2.Tests/dhbw.WebEngineering.V2.Tests.csproj", "dhbw.WebEngineering.V2.Tests/"]

# Restore dependencies based on architecture to cache packages per platform
ARG TARGETARCH
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore "dhbw.WebEngineering.V2.Api/dhbw.WebEngineering.V2.Api.csproj" --arch ${TARGETARCH/amd64/x64}

# Copy the entire source
COPY . .

# Set the working directory to the project folder
WORKDIR /source/dhbw.WebEngineering.V2.Api

# Build and publish the application
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --arch ${TARGETARCH/amd64/x64}

################################################################################
# Stage 2: Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final

# Set up working directory
WORKDIR /app

# Copy published application from the build stage
COPY --from=build /app/publish .

# Set up necessary permissions and expose port
USER root
EXPOSE 8080

# Start the application
ENTRYPOINT ["dotnet", "dhbw.WebEngineering.V2.Api.dll"]