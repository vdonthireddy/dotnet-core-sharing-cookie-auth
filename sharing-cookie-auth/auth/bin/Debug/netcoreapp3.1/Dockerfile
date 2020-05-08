FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY auth.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c release -o /output

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /output .
ENV ASPNETCORE_URLS=http://+:8435
ENTRYPOINT ["dotnet", "auth.dll"]
