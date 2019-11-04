FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["CoreDemo01/CoreDemo01.csproj", "CoreDemo01/"]
RUN dotnet restore "CoreDemo01/CoreDemo01.csproj"
COPY . .
WORKDIR "/src/CoreDemo01"
RUN dotnet build "CoreDemo01.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoreDemo01.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoreDemo01.dll"]