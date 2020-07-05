#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["MyBlog.Web/MyBlog.Web.csproj", "MyBlog.Web/"]
COPY ["MyBlog.Core/MyBlog.Core.csproj", "MyBlog.Core/"]
COPY ["MyBlog.Infrastructure/MyBlog.Infrastructure.csproj", "MyBlog.Infrastructure/"]
RUN dotnet restore "MyBlog.Web/MyBlog.Web.csproj"
COPY . .
WORKDIR "/src/MyBlog.Web"
RUN dotnet build "MyBlog.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyBlog.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyBlog.Web.dll"]