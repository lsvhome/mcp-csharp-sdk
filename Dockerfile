#FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
#USER $APP_UID
#WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release

RUN apt update \
    && apt install -y build-essential clang zlib1g-dev

#WORKDIR /src
#COPY ["samples/AspNetCoreMcpServer/AspNetCoreMcpServer.csproj", "samples/AspNetCoreMcpServer/"]
#COPY ["src/ModelContextProtocol.AspNetCore/ModelContextProtocol.AspNetCore.csproj", "src/ModelContextProtocol.AspNetCore/"]
#COPY ["src/ModelContextProtocol/ModelContextProtocol.csproj", "src/ModelContextProtocol/"]
#COPY ["src/ModelContextProtocol.Core/ModelContextProtocol.Core.csproj", "src/ModelContextProtocol.Core/"]
#COPY ["src/ModelContextProtocol.Analyzers/ModelContextProtocol.Analyzers.csproj", "src/ModelContextProtocol.Analyzers/"]
COPY . .
RUN dotnet restore 
#"samples/AspNetCoreMcpServer/AspNetCoreMcpServer.csproj"

#RUN dotnet build -c $BUILD_CONFIGURATION "samples/AspNetCoreMcpServer/AspNetCoreMcpServer.csproj"

#WORKDIR "/samples/AspNetCoreMcpServer"

#RUN dotnet test -c $BUILD_CONFIGURATION -o /app/build
RUN pwd
RUN ls . -1al
RUN dotnet publish  -f net10.0 -o /app/publish /p:UseAppHost=false /samples/AspNetCoreMcpServer/AspNetCoreMcpServer.csproj
#-c $BUILD_CONFIGURATION 

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /
#RUN mkdir /app
#WORKDIR /app
COPY --from=build /app/publish /app/

EXPOSE 8080
EXPOSE 8081

RUN ls /app -1al

#ENTRYPOINT ["dotnet", "/app/AspNetCoreMcpServer.dll"]
ENTRYPOINT ["/app/AspNetCoreMcpServer"]
