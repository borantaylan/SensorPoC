# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["SensorPoC.sln", "./"]
COPY ["SensorPoC.WebAPI/SensorPoC.WebAPI.csproj", "SensorPoC.WebAPI/"]
COPY ["SensorPoC.Domain/SensorPoC.Domain.csproj", "SensorPoC.Domain/"]
COPY ["SensorPoC.Storage/SensorPoC.Storage.csproj", "SensorPoC.Storage/"]
COPY ["ImaginaryCompany.SensorDataProvider/ImaginaryCompany.SensorDataProvider.csproj", "ImaginaryCompany.SensorDataProvider/"]

# Restore dependencies
RUN dotnet restore "SensorPoC.WebAPI/SensorPoC.WebAPI.csproj"

# Copy the entire solution
COPY . .

# Build the WebAPI project
WORKDIR /src/SensorPoC.WebAPI
RUN dotnet build -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .

# Expose the WebAPI port
EXPOSE 80

# Entry point for the WebAPI
ENTRYPOINT ["dotnet", "SensorPoC.WebAPI.dll"]