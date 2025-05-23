FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy only the .csproj first
COPY ["testDeployGCP.csproj", "./"]

# Restore dependencies
RUN dotnet restore "./testDeployGCP.csproj"

# Copy the rest of the source code
COPY . .

# Change into the project directory
WORKDIR "/src/testDeployGCP"

# Build
RUN dotnet build "./testDeployGCP.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "testDeployGCP.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "testDeployGCP.dll"]