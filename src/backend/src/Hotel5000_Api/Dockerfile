#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# NuGet restore
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY *.sln .
COPY Core/*.csproj Core/
COPY Infrastructure/*.csproj Infrastructure/
COPY Web/*.csproj Web/
RUN dotnet restore
COPY . .

# publish
FROM build AS publish
WORKDIR /src/Web
RUN dotnet publish -c Release -o /src/publish

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=publish /src/publish .
# ENTRYPOINT ["dotnet", "Colors.API.dll"]
# heroku uses the following
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Colors.API.dll

#FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
#WORKDIR /src
#COPY ["Web/Web.csproj", "Web/"]
#COPY ["Core/Core.csproj", "Core/"]
#COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
#RUN dotnet restore "Web/Web.csproj"
#COPY . .
#WORKDIR "/src/Web"
#RUN dotnet build "Web.csproj" -c Release -o /app/build

#FROM build AS publish
#RUN dotnet publish "Web.csproj" -c Release -o /app/publish

#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Web.dll"]