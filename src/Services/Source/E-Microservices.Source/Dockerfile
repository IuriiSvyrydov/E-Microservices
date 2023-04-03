#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Services/Source/E-Microservices.Source/E-Microservices.Source.csproj", "src/Services/Source/E-Microservices.Source/"]
COPY ["src/Services/Common/EventBusRabbitMQ/EventBusRabbitMQ.csproj", "src/Services/Common/EventBusRabbitMQ/"]
RUN dotnet restore "src/Services/Source/E-Microservices.Source/E-Microservices.Source.csproj"
COPY . .
WORKDIR "/src/src/Services/Source/E-Microservices.Source"
RUN dotnet build "E-Microservices.Source.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "E-Microservices.Source.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "E-Microservices.Source.dll"]