FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["src/Constriva.API/Constriva.API.csproj", "src/Constriva.API/"]
COPY ["src/Constriva.Application/Constriva.Application.csproj", "src/Constriva.Application/"]
COPY ["src/Constriva.Domain/Constriva.Domain.csproj", "src/Constriva.Domain/"]
COPY ["src/Constriva.Infrastructure/Constriva.Infrastructure.csproj", "src/Constriva.Infrastructure/"]
COPY ["src/Constriva.Messaging.Contracts/Constriva.Messaging.Contracts.csproj", "src/Constriva.Messaging.Contracts/"]

# Restore
RUN dotnet restore "src/Constriva.API/Constriva.API.csproj"

# Copy source
COPY . .

# Build
WORKDIR "/src/src/Constriva.API"
RUN dotnet build "Constriva.API.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "Constriva.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
EXPOSE 5000
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Constriva.API.dll"]
