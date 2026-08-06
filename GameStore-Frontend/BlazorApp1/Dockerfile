FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["GameStore-Frontend/BlazorApp1/BlazorApp1.csproj", "GameStore-Frontend/BlazorApp1/"]
RUN dotnet restore "GameStore-Frontend/BlazorApp1/BlazorApp1.csproj"

COPY . .
WORKDIR "/src/GameStore-Frontend/BlazorApp1"
RUN dotnet publish "BlazorApp1.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "BlazorApp1.dll"]
