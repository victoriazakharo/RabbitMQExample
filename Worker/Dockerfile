FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /worker

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/runtime:2.2
WORKDIR /worker
COPY --from=build-env /worker/out .
ENTRYPOINT ["dotnet", "Worker.dll"]