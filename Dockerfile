FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

RUN adduser -u 32769 -m -U dokku
USER dokku

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["*.sln", "./"]
COPY ["Persistence/Persistence.csproj", "Persistence/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["API/API.csproj", "API/"]
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet API.dll