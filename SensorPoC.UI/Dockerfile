# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["SensorPoC.sln", "./"]
COPY ["SensorPoC.UI/SensorPoC.UI.csproj", "SensorPoC.UI/"]

# Restore dependencies
RUN dotnet restore "SensorPoC.UI/SensorPoC.UI.csproj"

# Copy the entire solution
COPY . .

# Build the UI project
WORKDIR /src/SensorPoC.UI
RUN dotnet build -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .

# Expose the UI port
EXPOSE 80

# Entry point for the UI
ENTRYPOINT ["dotnet", "SensorPoC.UI.dll"]