#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
RUN apt-get update
RUN apt-get install --yes curl
RUN curl --silent --location https://deb.nodesource.com/setup_10.x | bash -
RUN apt-get install --yes nodejs

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
RUN apt-get update
RUN apt-get install --yes curl
RUN curl --silent --location https://deb.nodesource.com/setup_10.x | bash -
RUN apt-get install --yes nodejs
WORKDIR /src
COPY ["Logcast.Recruitment.Web/Logcast.Recruitment.Web.csproj", "Logcast.Recruitment.Web/"]
COPY ["Logcast.Recruitment.DataAccess/Logcast.Recruitment.DataAccess.csproj", "Logcast.Recruitment.DataAccess/"]
COPY ["Logcast.Recruitment.Shared/Logcast.Recruitment.Shared.csproj", "Logcast.Recruitment.Shared/"]
COPY ["Logcast.Recruitment.Domain/Logcast.Recruitment.Domain.csproj", "Logcast.Recruitment.Domain/"]
RUN dotnet restore "Logcast.Recruitment.Web/Logcast.Recruitment.Web.csproj"
COPY . .
WORKDIR "/src/Logcast.Recruitment.Web"
RUN dotnet build "Logcast.Recruitment.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Logcast.Recruitment.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Logcast.Recruitment.Web.dll"]