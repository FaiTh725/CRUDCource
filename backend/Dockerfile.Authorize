FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

WORKDIR /src
COPY ["Authorize/Authorize.csproj", "Authorize/"]

RUN dotnet restore 'Authorize/Authorize.csproj'

COPY . .

WORKDIR /src/Authorize
RUN dotnet build -c Release -o /app/build

FROM build as publish
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 
ENV ASPNETCORE_HTTP_PORTS=5101
ENV ASPNETCORE_HTTPS_PORTS=5102
EXPOSE 5101
EXPOSE 5102
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "Authorize.dll" ]