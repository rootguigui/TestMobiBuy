#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443
EXPOSE 5006

ENV ASPNETCORE_URLS=http://+:5006 \
    ASPNETCORE_ENVIRONMENT=Production

# RUN
RUN apk add --no-cache icu-data-full icu-libs tzdata krb5-libs libgcc libintl libssl3 libstdc++ zlib

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /build

ARG BUILD_CONFIGURATION=Release
ENV BUILD_CONFIGURATION=${BUILD_CONFIGURATION}

COPY ["src/TestMobiBuy.Api/TestMobiBuy.Api.csproj", "src/TestMobiBuy.Api/"]
COPY ["src/TestMobiBuy.Application/TestMobiBuy.Application.csproj", "src/TestMobiBuy.Application/"]
COPY ["src/TestMobiBuy.Infrastructure/TestMobiBuy.Infrastructure.csproj", "src/TestMobiBuy.Infrastructure/"]
COPY ["src/TestMobiBuy.Domain/TestMobiBuy.Domain.csproj", "src/TestMobiBuy.Domain/"]
COPY ["src/TestMobiBuy.Share/TestMobiBuy.Share.csproj", "src/TestMobiBuy.Share/"]
RUN dotnet restore -s https://api.nuget.org/v3/index.json "src/TestMobiBuy.Api/TestMobiBuy.Api.csproj"

COPY . .
WORKDIR "/build/src/TestMobiBuy.Api"
RUN dotnet build "TestMobiBuy.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestMobiBuy.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestMobiBuy.Api.dll"]