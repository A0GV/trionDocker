FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ApiGame/*.csproj ./ApiGame/
RUN dotnet restore ApiGame/ApiGame.csproj

# Copy everything else and build
COPY . ./
RUN dotnet publish ApiGame/ApiGame.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
COPY ApiGame/ca.pem ./ca.pem

# Expose port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Run the app
ENTRYPOINT ["dotnet", "ApiGame.dll"]