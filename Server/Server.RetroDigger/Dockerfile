# Multi-stage build for ASP.NET Core API
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Server.RetroDigger/Server.RetroDigger.csproj", "Server.RetroDigger/"]
RUN dotnet restore "Server.RetroDigger/Server.RetroDigger.csproj"

COPY . .
WORKDIR "/src/Server.RetroDigger"
RUN dotnet publish "Server.RetroDigger.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV SCORES_PATH=/data/scores.json

RUN mkdir /data
VOLUME ["/data"]

COPY --from=build /app/publish .
EXPOSE 8080

ENTRYPOINT ["dotnet", "Server.RetroDigger.dll"]
