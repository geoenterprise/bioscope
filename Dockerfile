# --- build stage ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy csproj and restore first (better layer caching)
COPY Api/Api.csproj Api/
RUN dotnet restore Api/Api.csproj

# copy the rest and publish
COPY Api/ Api/
RUN dotnet publish Api/Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# --- runtime stage ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Render routes traffic to $PORT; bind Kestrel to it
ENV ASPNETCORE_URLS=http://0.0.0.0:10000
EXPOSE 10000

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]
