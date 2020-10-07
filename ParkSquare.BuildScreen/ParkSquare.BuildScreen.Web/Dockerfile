FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM base AS final
WORKDIR /app
COPY ParkSquare.BuildScreen.Web/. /app
ENTRYPOINT ["dotnet", "ParkSquare.BuildScreen.Web.dll"]
