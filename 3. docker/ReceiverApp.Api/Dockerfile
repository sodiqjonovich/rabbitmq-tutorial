FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /ReceiverApp.Api
COPY ReceiverApp.Api/ .
RUN dotnet restore "ReceiverApp.Api/ReceiverApp.Api.csproj"
RUN dotnet build "ReceiverApp.Api/ReceiverApp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ReceiverApp.Api/ReceiverApp.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReceiverApp.Api.dll"]
