# Use .NET 8 SDK to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY TrackItAPI.csproj .
RUN dotnet restore "TrackItAPI.csproj"

# Copy everything else and build
COPY . .
RUN dotnet publish "TrackItAPI.csproj" -c Release -o /app/publish

# Use .NET 8 runtime image for final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TrackItAPI.dll"]
