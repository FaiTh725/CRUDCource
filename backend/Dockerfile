FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

WORKDIR /src
COPY ["Main/Main.csproj", "Main/"]

RUN dotnet restore 'Main/Main.csproj'

COPY . .

WORKDIR /src/Main
RUN dotnet build -c Release -o /app/build

FROM build as publish
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 
ENV ASPNETCORE_HTTP_PORTS=5401
ENV ASPNETCORE_HTTPS_PORTS=5402
EXPOSE 5401
EXPOSE 5402
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "Main.dll" ]