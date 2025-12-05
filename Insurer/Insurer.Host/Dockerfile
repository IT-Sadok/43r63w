# =======================
#  Base image for runtime
# =======================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# =======================
#  Build stage
# =======================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# Copy solution-level directories
COPY ./Insurer.Host/Insurer.Host.csproj ./Insurer.Host/
COPY ./Modules ./Modules
COPY ./Shared ./Shared

# Restore only the host project (loads deps from modules)
RUN dotnet restore ./Insurer.Host/Insurer.Host.csproj

# Copy entire project after restoring
COPY . .

# Build
RUN dotnet build ./Insurer.Host/Insurer.Host.csproj \
    -c $BUILD_CONFIGURATION \
    -o /app/build

# =======================
#  Publish stage
# =======================
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish ./Insurer.Host/Insurer.Host.csproj \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# =======================
# Final runtime image
# =======================
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Insurer.Host.dll"]
