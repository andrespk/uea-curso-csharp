# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["MiniKanban.API/MiniKanban.API.csproj", "MiniKanban.API/"]
COPY ["MiniKanban.Application/MiniKanban.Application.csproj", "MiniKanban.Application/"]
COPY ["MiniKanban.Domain/MiniKanban.Domain.csproj", "MiniKanban.Domain/"]
COPY ["MiniKanban.Infrastructure/MiniKanban.Infrastructure.csproj", "MiniKanban.Infrastructure/"]
COPY ["MiniKanban.Exceptions/MiniKanban.Exceptions.csproj", "MiniKanban.Exceptions/"]

RUN dotnet restore "MiniKanban.API/MiniKanban.API.csproj"
COPY . .

WORKDIR "/src/MiniKanban.API"
RUN dotnet build "MiniKanban.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MiniKanban.API.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080
ENTRYPOINT ["dotnet", "MiniKanban.API.dll"]
