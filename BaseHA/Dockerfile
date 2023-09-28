#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BaseHA/BaseHA.csproj", "BaseHA/"]
COPY ["BaseHA.Domain/BaseHA.Domain.csproj", "BaseHA.Domain/"]
COPY ["BaseHA/Share.BaseCore/Share.BaseCore.csproj", "Share.BaseCore/"]
RUN dotnet restore "BaseHA/BaseHA.csproj"
COPY . .
WORKDIR "/src/BaseHA"
RUN dotnet build "BaseHA.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BaseHA.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BaseHA.dll"]