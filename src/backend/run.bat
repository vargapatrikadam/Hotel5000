SET ASPNETCORE_ENVIROMENT = Development
dotnet dev-certs https --trust
dotnet restore ./src/Hotel5000_Api/Web/Web.csproj
dotnet run --project ./src/Hotel5000_Api/Web/Web.csproj --launch-profile Dev --output ./bin --force
pause