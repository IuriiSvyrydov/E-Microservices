#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Services/Products/Services.Product/Services.Product.csproj", "src/Services/Products/Services.Product/"]
RUN dotnet restore "src/Services/Products/Services.Product/Services.Product.csproj"
COPY . .
WORKDIR "/src/src/Services/Products/Services.Product"
RUN dotnet build "Services.Product.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Services.Product.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Services.Product.dll"]