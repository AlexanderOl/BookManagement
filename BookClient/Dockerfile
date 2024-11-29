FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /app

COPY ["BookClient/BookClient.csproj", "BookClient/"]
RUN dotnet restore "BookClient/BookClient.csproj"

COPY . ./
RUN dotnet publish -c Release -o /out

FROM nginx:alpine AS runtime

COPY --from=build /out/wwwroot /usr/share/nginx/html

EXPOSE 7172

# Start NGINX in the foreground
ENTRYPOINT ["nginx", "-g", "daemon off;"]