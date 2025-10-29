# Use the official .NET SDK as a base image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory in the container
WORKDIR /app

# Copy the .csproj file and restore dependencies
COPY TrackIt-API/TrackIt-API.csproj ./TrackIt-API/

# Restore the .NET dependencies
RUN dotnet restore ./TrackIt-API/TrackIt-API.csproj

# Copy the rest of the code
COPY . .

# Publish the application to the /out folder in the container
RUN dotnet publish ./TrackIt-API/TrackIt-API.csproj -c Release -o /app/publish

# Set the base image for the final image and copy the published app
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
COPY --from=build /app/publish .

# Expose the port
EXPOSE 80

# Define the entry point for the application
ENTRYPOINT ["dotnet", "TrackIt-API.dll"]
