FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/DeveloperStore.Sales.API/DeveloperStore.Sales.API.csproj", "src/DeveloperStore.Sales.API/"]
COPY ["src/DeveloperStore.Sales.Application/DeveloperStore.Sales.Application.csproj", "src/DeveloperStore.Sales.Application/"]
COPY ["src/DeveloperStore.Sales.Domain/DeveloperStore.Sales.Domain.csproj", "src/DeveloperStore.Sales.Domain/"]
COPY ["src/DeveloperStore.Sales.Infrastructure/DeveloperStore.Sales.Infrastructure.csproj", "src/DeveloperStore.Sales.Infrastructure/"]
RUN dotnet restore "src/DeveloperStore.Sales.API/DeveloperStore.Sales.API.csproj"
COPY . .
WORKDIR "/src/src/DeveloperStore.Sales.API"
RUN dotnet build "DeveloperStore.Sales.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DeveloperStore.Sales.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeveloperStore.Sales.API.dll"]
