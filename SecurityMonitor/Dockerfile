# Используем .NET Core SDK образ для сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копируем csproj и восстанавливаем зависимости
COPY SecurityMonitor.csproj .
RUN dotnet restore

# Копируем и собираем приложение
COPY . .
RUN dotnet publish -c Release -o out

# Используем .NET Core Runtime образ для запуска приложения
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "SecurityMonitor.dll"]
