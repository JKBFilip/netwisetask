FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["CatFactSolution.sln", "./"]
COPY ["src/CatFactService.Core/CatFactService.Core.csproj", "src/CatFactService.Core/"]
COPY ["src/CatFactService.Infrastructure/CatFactService.Infrastructure.csproj", "src/CatFactService.Infrastructure/"]
COPY ["src/CatFactService.Worker/CatFactService.Worker.csproj", "src/CatFactService.Worker/"]
RUN dotnet restore "src/CatFactService.Worker/CatFactService.Worker.csproj"

COPY . .
WORKDIR "/src/src/CatFactService.Worker"
RUN dotnet publish "CatFactService.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "CatFactService.Worker.dll"]