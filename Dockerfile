FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia todos os projetos necessários
COPY HealthMed.API/HealthMed.Access.API.csproj HealthMed.API/
COPY Common/HealthMed.API.Access.Common.csproj Common/
COPY HealthMed.Business/HealthMed.Access.Business.csproj HealthMed.Business/
COPY HealthMed.Domain/HealthMed.Access.Domain.csproj HealthMed.Domain/
COPY HealthMed.Infrastructure/HealthMed.Access.Infrastructure.csproj HealthMed.Infrastructure/

# Copia a solução e restaura
COPY HealthMedSolution.Access.sln .
RUN dotnet restore "HealthMed.API/HealthMed.Access.API.csproj"

# Copia todo o restante
COPY . .

# Build do projeto da API
WORKDIR "/src/HealthMed.API"
RUN dotnet build "HealthMed.Access.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HealthMed.Access.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HealthMed.Access.API.dll"]
