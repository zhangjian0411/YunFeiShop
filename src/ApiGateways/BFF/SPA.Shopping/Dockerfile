# FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
FROM dotnet-aspnet-with-devcerts:5.0-r0 AS base
WORKDIR /app
EXPOSE 15000
ENV ASPNETCORE_URLS=http://*:15000

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SPA.Shopping.csproj", "./"]
RUN dotnet restore "./SPA.Shopping.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "SPA.Shopping.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SPA.Shopping.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SPA.Shopping.dll"]
