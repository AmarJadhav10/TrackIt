FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy project file and restore
COPY TrackItAPI.csproj .
RUN dotnet restore "TrackItAPI.csproj"

# Copy rest of the code
COPY . .
RUN dotnet publish "TrackItAPI.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TrackItAPI.dll"]
