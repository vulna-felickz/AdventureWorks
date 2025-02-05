FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Services/ReferenceData/src/AW.Services.ReferenceData.REST.API/AW.Services.ReferenceData.REST.API.csproj", "Services/ReferenceData/src/AW.Services.ReferenceData.REST.API/"]
COPY aw-ca.pem .
RUN dotnet restore "Services/ReferenceData/src/AW.Services.ReferenceData.REST.API/AW.Services.ReferenceData.REST.API.csproj"
COPY . .
WORKDIR "/src/Services/ReferenceData/src/AW.Services.ReferenceData.REST.API"
RUN dotnet build "AW.Services.ReferenceData.REST.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AW.Services.ReferenceData.REST.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /src/aw-ca.pem /etc/ssl/certs/aw-ca.pem
ENTRYPOINT ["dotnet", "AW.Services.ReferenceData.REST.API.dll"]