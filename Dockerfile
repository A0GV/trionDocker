FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["ApiGame/ApiGame.csproj", "ApiGame/"]
RUN dotnet restore "ApiGame/ApiGame.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "ApiGame/ApiGame.csproj" -c Release -o /app/build
RUN dotnet publish "ApiGame/ApiGame.csproj" -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published app and certificate
COPY --from=build /app/publish .
COPY ["ApiGame/ca.pem", "ca.pem"]

# Make sure certificate is accessible
RUN chmod 644 ca.pem

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose the port
EXPOSE 8080

# Set the entry point
ENTRYPOINT ["dotnet", "ApiGame.dll"]