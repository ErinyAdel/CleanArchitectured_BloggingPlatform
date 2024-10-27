FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY ["BloggingPlatform.WebAPI/BloggingPlatform.WebAPI.csproj", "BloggingPlatform.WebAPI/"]
COPY ["BloggingPlatform.Application/BloggingPlatform.Application.csproj", "BloggingPlatform.Application/"]
COPY ["BloggingPlatform.Persistence/BloggingPlatform.Persistence.csproj", "BloggingPlatform.Persistence/"]
COPY ["BloggingPlatform.Domain/BloggingPlatform.Domain.csproj", "BloggingPlatform.Domain/"]
RUN dotnet restore "BloggingPlatform.WebAPI/BloggingPlatform.WebAPI.csproj"
COPY . .
WORKDIR /app/BloggingPlatform.WebAPI
RUN dotnet build "BloggingPlatform.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BloggingPlatform.WebAPI.dll"]