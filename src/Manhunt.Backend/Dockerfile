# Datei: Manhunt.Backend/Dockerfile

# 1. Build-Stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Manhunt.Backend.csproj", "./"]
RUN dotnet restore "./Manhunt.Backend.csproj"
COPY . .
RUN dotnet publish "Manhunt.Backend.csproj" -c Release -o /app/publish

# 2. Laufzeit-Stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Manhunt.Backend.dll"]
