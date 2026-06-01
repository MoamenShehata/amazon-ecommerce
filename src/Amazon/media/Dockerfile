FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Media.Api/Media.Api.csproj", "Media.Api/"]
COPY ["Media.Application/Media.Application.csproj", "Media.Application/"]
COPY ["Media.Domain/Media.Domain.csproj", "Media.Domain/"]
COPY ["Media.Infrastructure/Media.Infrastructure.csproj", "Media.Infrastructure/"]
RUN dotnet restore "./Media.Api/Media.Api.csproj"
COPY . .
WORKDIR "/src/Media.Api"
RUN dotnet build "./Media.Api.csproj" -o /app/build

FROM build AS publish
RUN dotnet publish "./Media.Api.csproj" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Media.Api.dll"]