FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY WrightToPdf.csproj WrightToPdf/
RUN dotnet restore "WrightToPdf/WrightToPdf.csproj"
COPY . WrightToPdf/
WORKDIR "/src/WrightToPdf"
RUN dotnet build "WrightToPdf.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WrightToPdf.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WrightToPdf.dll"]