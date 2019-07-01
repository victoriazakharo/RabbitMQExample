FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY ["Sender/Sender.csproj", "Sender/"]
RUN dotnet restore "Sender/Sender.csproj"
COPY . .
WORKDIR "/src/Sender"
RUN dotnet build "Sender.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Sender.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Sender.dll"]