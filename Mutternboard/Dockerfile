# Basis-Image für den Betrieb (ASP.NET-Image)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build-Image (SDK-Image zum Bauen des Projekts)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Kopiere das Projekt und führe `dotnet restore` aus
COPY ["Mutternboard.csproj", "Mutternboard/"]
WORKDIR "/src/Mutternboard"
RUN dotnet restore "Mutternboard.csproj"

# Kopiere den gesamten Quellcode und baue das Projekt
COPY . .
RUN dotnet build "Mutternboard.csproj" -c Release -o /app/build

# Publizieren der Anwendung
FROM build AS publish
RUN dotnet publish "Mutternboard.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Endgültiges Image (nur das veröffentlichte Projekt kopieren)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Definiere den Einstiegspunkt (ENTRYPOINT)
ENTRYPOINT ["dotnet", "Mutternboard.dll"]
