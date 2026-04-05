FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/Constriva.Messaging/Constriva.Messaging.csproj", "src/Constriva.Messaging/"]
COPY ["src/Constriva.Messaging.Contracts/Constriva.Messaging.Contracts.csproj", "src/Constriva.Messaging.Contracts/"]

RUN dotnet restore "src/Constriva.Messaging/Constriva.Messaging.csproj"

COPY . .

WORKDIR "/src/src/Constriva.Messaging"
RUN dotnet build "Constriva.Messaging.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Constriva.Messaging.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Constriva.Messaging.dll"]
