FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["UI/Store/src/AW.UI.Web.Store/AW.UI.Web.Store.csproj", "UI/AW.UI.Web.Store/"]
COPY aw-ca.pem .
RUN dotnet restore "UI/AW.UI.Web.Store/AW.UI.Web.Store.csproj"
COPY . .
WORKDIR "/src/UI/Store/src/AW.UI.Web.Store"
RUN dotnet build "AW.UI.Web.Store.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AW.UI.Web.Store.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /src/aw-ca.pem /etc/ssl/certs/aw-ca.pem
ENTRYPOINT ["dotnet", "AW.UI.Web.Store.dll"]