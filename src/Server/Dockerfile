FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["FestoVideoStream.csproj", ""]
RUN dotnet restore "FestoVideoStream.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "FestoVideoStream.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "FestoVideoStream.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "FestoVideoStream.dll"]