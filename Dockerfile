# Use the .NET 6.0 SDK as the base image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Set the working directory
WORKDIR /app

# Copy the project files to the working directory
COPY . .

# Restore dependencies
RUN dotnet restore

# Build the application
RUN dotnet build -c Release --no-restore

# Publish the application
RUN dotnet publish -c Release --no-build -o /app/publish -r linux-x64
RUN dotnet publish -c Release --no-build -o /app/publish

# Use the .NET 6.0 runtime as the base image for the final image
FROM mcr.microsoft.com/dotnet/runtime:6.0 AS final

# Set the working directory
WORKDIR /app

# Copy the published files from the build image to the final image
COPY --from=build /app/publish .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "NET-Hack-2023.dll"]
# Set the entry point for the application
ENTRYPOINT ["dotnet", "NET-Hack-2023.dll"]
