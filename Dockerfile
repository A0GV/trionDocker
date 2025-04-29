FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy and restore project files
COPY ["ApiGame/ApiGame.csproj", "ApiGame/"]
RUN dotnet restore "ApiGame/ApiGame.csproj"

# Copy the rest and publish
COPY . .
RUN dotnet publish "ApiGame/ApiGame.csproj" -c Release -o /app/publish

# Final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
COPY ["ApiGame/ca.pem", "./ca.pem"]

# Environment variables
ENV ASPNETCORE_URLS=http://+:8080

# Expose port
EXPOSE 8080

# Start the app
ENTRYPOINT ["dotnet", "ApiGame.dll"]