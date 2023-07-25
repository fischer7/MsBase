#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Fischer.Audit/Fischer.Audit.csproj", "Fischer.Audit/"]
COPY ["src/Fischer.Core/Fischer.Core.csproj", "Fischer.Core/"]
RUN dotnet restore "Fischer.Audit/Fischer.Audit.csproj"
COPY . .
WORKDIR "Fischer.Audit"
RUN dotnet build "Fischer.Audit.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fischer.Audit.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fischer.Audit.dll"]