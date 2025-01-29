FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

WORKDIR /src
COPY ["Message/Message.csproj", "Message/"]

RUN dotnet restore 'Message/Message.csproj'

COPY . .

WORKDIR /src/Message
RUN dotnet build -c Release -o /app/build

FROM build as publish
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 
ENV ASPNETCORE_HTTP_PORTS=5301
ENV ASPNETCORE_HTTPS_PORTS=5302
EXPOSE 5301
EXPOSE 5302
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "Message.dll" ]
