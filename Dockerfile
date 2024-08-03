# Use the official .NET 8 SDK image as a build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory
WORKDIR /app

# Copy the project file and restore dependencies
COPY DomeneShop.CLI/DomeneShop.CLI.csproj ./DomeneShop.CLI/
RUN dotnet restore DomeneShop.CLI/DomeneShop.CLI.csproj

# Copy the rest of the application code
COPY . .

# Build the application
RUN dotnet publish DomeneShop.CLI/DomeneShop.CLI.csproj -c Release -o out

# Use the official .NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/runtime:8.0

# Set the working directory
WORKDIR /app

# Copy the build output to the runtime image
COPY --from=build /app/out .

# Specify the entry point for the application
ENTRYPOINT ["dotnet", "DomeneShop.CLI.dll"]
