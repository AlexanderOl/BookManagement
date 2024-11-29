FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["BookServer/BookServer.csproj", "BookServer/"]
RUN dotnet restore "BookServer/BookServer.csproj"

COPY . .
WORKDIR "/src/BookServer"
RUN dotnet build "BookServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookServer.dll"]