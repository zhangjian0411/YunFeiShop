FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://*

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
# COPY ["BuildingBlocks/IntegrationEvents/IntegrationEvents.Abstractions/IntegrationEvents.Abstractions.csproj", "BuildingBlocks/IntegrationEvents/IntegrationEvents.Abstractions/IntegrationEvents.Abstractions.csproj"]
# COPY ["BuildingBlocks/IntegrationEvents/IntegrationEvents/IntegrationEvents.csproj", "BuildingBlocks/IntegrationEvents/IntegrationEvents/IntegrationEvents.csproj"]
# COPY ["BuildingBlocks/IntegrationEvents/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj", "BuildingBlocks/IntegrationEvents/EventBus.RabbitMQ/EventBus.RabbitMQ.csproj"]
# COPY ["BuildingBlocks/SeedWork/SeedWork.csproj", "BuildingBlocks/SeedWork/SeedWork.csproj"]
# COPY ["Services/Carts/Carts.API/Carts.API.csproj", "Services/Carts/Carts.API/Carts.API.csproj"]
COPY . .
RUN dotnet restore "Services/Carts/Carts.API/Carts.API.csproj"
# COPY . .
RUN dotnet build "Services/Carts/Carts.API/Carts.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Services/Carts/Carts.API/Carts.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Carts.API.dll"]