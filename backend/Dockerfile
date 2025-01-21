FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

WORKDIR /src
COPY ["Product/Product.csproj", "Product/"]

RUN dotnet restore 'Product/Product.csproj'

COPY . .

WORKDIR /src/Product
RUN dotnet build -c Release -o /app/build

FROM build as publish
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 
ENV ASPNETCORE_HTTP_PORTS=5201
ENV ASPNETCORE_HTTPS_PORTS=5202
EXPOSE 5201
EXPOSE 5202
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "Product.dll" ]