FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["docker-worker-net6.csproj", "./"]
RUN dotnet restore "docker-worker-net6.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "docker-worker-net6.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "docker-worker-net6.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "docker-worker-net6.dll"]
