FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
EXPOSE 80
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./Presentation/OrderApp.API/OrderApp.API.csproj", "Presentation/OrderApp.API/"]
COPY ["./Infrastructure/OrderApp.Persistence/OrderApp.Persistence.csproj", "Infrastructure/OrderApp.Persistence/"]
COPY ["./Infrastructure/OrderApp.Infrastructure/OrderApp.Infrastructure.csproj", "Infrastructure/OrderApp.Infrastructure/"]
COPY ["./Core/OrderApp.Domain/OrderApp.Domain.csproj", "Core/OrderApp.Domain/"]
COPY ["./Core/OrderApp.Application/OrderApp.Application.csproj", "Core/OrderApp.Application/"]

RUN dotnet restore "./Presentation\OrderApp.API\OrderApp.API.csproj"

COPY . .
WORKDIR "/src/Presentation/OrderApp.API"
RUN dotnet build "OrderApp.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderApp.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet OrderApp.API.dll