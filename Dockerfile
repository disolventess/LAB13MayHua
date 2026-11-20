FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY Lab13_JoseTincopa/ ./Lab13_JoseTincopa/
RUN dotnet publish Lab13_JoseTincopa/Lab13_JoseTincopa.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish ./
ENV ASPNETCORE_URLS=http://0.0.0.0:10000
ENTRYPOINT ["dotnet", "Lab13_JoseTincopa.dll"]