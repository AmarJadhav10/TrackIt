# Use .NET 6 SDK to build the app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy the .csproj file and restore dependencies
COPY TrackItAPI/TrackItAPI.csproj TrackItAPI/
RUN dotnet restore "TrackItAPI/TrackItAPI.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/TrackItAPI"
RUN dotnet build "TrackItAPI.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "TrackItAPI.csproj" -c Release -o /app/publish

# Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "TrackItAPI.dll"]
