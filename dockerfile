# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim as build-env
WORKDIR /LoyaltyServices
COPY . .
RUN dotnet restore
COPY LoyaltyServices .
RUN dotnet publish -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim as runtime
WORKDIR /publish
COPY --from=build-env /publish .

EXPOSE 5112:80
ENV ASPNETCORE_URLS=http://+:5112
ENTRYPOINT ["dotnet", "LoyaltyServices.dll"]
CMD ["nginx", "-g", "daemon off;"]
