# =======================
#  Base image for runtime
# =======================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
EXPOSE 8081

# =======================
#  Build stage
# =======================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# Copy project files individually to improve caching
COPY Insurer.Host/Insurer.Host.csproj Insurer.Host/

COPY Modules/User/User.Infrastructure/User.Infrastructure.csproj Modules/User/User.Infrastructure/
COPY Modules/User/User.Domain/User.Domain.csproj Modules/User/User.Domain/
COPY Modules/User/User.Bootstrapper/User.Bootstrapper.csproj Modules/User/User.Bootstrapper/
COPY Modules/User/User.Application/User.Application.csproj Modules/User/User.Application/


COPY Modules/Policy/Policy.Domain/Policy.Domain.csproj Modules/Policy/Policy.Domain/
COPY Modules/Policy/Policy.Infrastructure/Policy.Infrastructure.csproj Modules/Policy/Policy.Infrastructure/
COPY Modules/Policy/Policy.Application/Policy.Application.csproj Modules/Policy/Policy.Application/
COPY Modules/Policy/Policy.Bootstrapper/Policy.Bootstrapper.csproj Modules/Policy/Policy.Bootstrapper/

COPY Shared/Shared/Shared.csproj Shared/Shared/

# Restore dependencies
RUN dotnet restore Insurer.Host/Insurer.Host.csproj

# Copy full source
COPY . .

# Build
RUN dotnet build Insurer.Host/Insurer.Host.csproj \
    -c $BUILD_CONFIGURATION \
    -o /app/build

# =======================
#  Publish stage
# =======================
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish Insurer.Host/Insurer.Host.csproj \
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
