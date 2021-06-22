# FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
FROM dotnet-aspnet-with-devcerts:5.0-r0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_URLS=http://*

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Identity.csproj", "./"]
RUN dotnet restore "./Identity.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.dll"]
