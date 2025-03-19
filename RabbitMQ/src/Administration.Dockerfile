# Prepare the build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution file and project files, then restore dependencies
COPY ./Pcf.Administration/*.sln .
COPY ./Pcf.Administration/*/*.csproj ./
# Copy the shared library project files
COPY ["./Pcf.Core.Integration/Pcf.Core.Integration.csproj", "Pcf.Core.Integration/"]
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*} && mv $file ${file%.*}; done
RUN dotnet restore

# Copy the remaining files and publish the application
COPY ./Pcf.Administration ./aspnetapp
# Copy the shared library source files
COPY ./Pcf.Core.Integration ./Pcf.Core.Integration

WORKDIR /app/aspnetapp
RUN dotnet publish -c Release -o out

# Use the .NET 8 runtime for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/aspnetapp/out ./
EXPOSE 8080
ENTRYPOINT ["dotnet", "Pcf.Administration.WebHost.dll"]