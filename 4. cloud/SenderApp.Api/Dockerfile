FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /SenderApp.Api
COPY SenderApp.Api/ .
RUN dotnet restore "SenderApp.Api/SenderApp.Api.csproj"
RUN dotnet build "SenderApp.Api/SenderApp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SenderApp.Api/SenderApp.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SenderApp.Api.dll"]