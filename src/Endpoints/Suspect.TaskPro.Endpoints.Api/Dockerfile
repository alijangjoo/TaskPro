# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Directory.Packages.props", "src/"]
COPY ["src/Endpoints/Suspect.TaskPro.Endpoints.Api/Suspect.TaskPro.Endpoints.Api.csproj", "src/Endpoints/Suspect.TaskPro.Endpoints.Api/"]
COPY ["Fx/src/Endpoints/OysterFx.Endpoints.Api/OysterFx.Endpoints.Api.csproj", "Fx/src/Endpoints/OysterFx.Endpoints.Api/"]
COPY ["Fx/src/AppCore/AppServices/OysterFx.AppCore.AppServices.csproj", "Fx/src/AppCore/AppServices/"]
COPY ["Fx/src/AppCore/Domain/OysterFx.AppCore.Domain.csproj", "Fx/src/AppCore/Domain/"]
COPY ["Fx/src/AppCore/Shared/OysterFx.AppCore.Shared.csproj", "Fx/src/AppCore/Shared/"]
COPY ["Fx/src/AppCore/DomainServices/OysterFx.AppCore.DomainServices.csproj", "Fx/src/AppCore/DomainServices/"]
COPY ["Fx/src/Infra/Persistence/RDB.Commands/OysterFx.Infra.Persistence.RDB.Commands.csproj", "Fx/src/Infra/Persistence/RDB.Commands/"]
COPY ["Fx/src/Infra/Auth/UserServices/OysterFx.Infra.Auth.UserServices.csproj", "Fx/src/Infra/Auth/UserServices/"]
COPY ["Fx/src/Infra/Persistence/EventSourcing.Abstractions/OysterFx.Infra.Persistence.EventSourcing.Abstractions.csproj", "Fx/src/Infra/Persistence/EventSourcing.Abstractions/"]
COPY ["Fx/src/Infra/Persistence/RDB.Queries/OysterFx.Infra.Persistence.RDB.Queries.csproj", "Fx/src/Infra/Persistence/RDB.Queries/"]
COPY ["Fx/src/Infra/Auth/OysterFx.Infra.Auth.JwtServices/OysterFx.Infra.Auth.JwtServices.csproj", "Fx/src/Infra/Auth/OysterFx.Infra.Auth.JwtServices/"]
COPY ["src/AppCore/AppServices/Suspect.TaskPro.AppCore.AppServices.csproj", "src/AppCore/AppServices/"]
COPY ["src/Infra/InternalServices/Supect.TaskPro.Infra.InternalServices.UserApiCaller/Supect.TaskPro.Infra.InternalServices.UserApiCaller.csproj", "src/Infra/InternalServices/Supect.TaskPro.Infra.InternalServices.UserApiCaller/"]
COPY ["Fx/src/Infra/ServiceCom/OysterFx.Infra.ServiceCom.ResutfulApi.Caller/OysterFx.Infra.ServiceCom.ResutfulApi.Caller.csproj", "Fx/src/Infra/ServiceCom/OysterFx.Infra.ServiceCom.ResutfulApi.Caller/"]
COPY ["src/Infra/PanelPayamak/Supect.TaskPro.Infra.PanelPayamak.Abstractions/Supect.TaskPro.Infra.PanelPayamak.Abstractions.csproj", "src/Infra/PanelPayamak/Supect.TaskPro.Infra.PanelPayamak.Abstractions/"]
COPY ["src/AppCore/Domain/Suspect.TaskPro.AppCore.Domain.csproj", "src/AppCore/Domain/"]
COPY ["src/AppCore/Shared/Suspect.TaskPro.AppCore.Shared.csproj", "src/AppCore/Shared/"]
COPY ["src/AppCore/DomainServices/Suspect.TaskPro.AppCore.DomainServices.csproj", "src/AppCore/DomainServices/"]
COPY ["src/Infra/PanelPayamak/Supect.TaskPro.Infra.PanelPayamak.KaveNegarProvider/Supect.TaskPro.Infra.PanelPayamak.KaveNegarProvider.csproj", "src/Infra/PanelPayamak/Supect.TaskPro.Infra.PanelPayamak.KaveNegarProvider/"]
COPY ["src/Infra/Persistence/Postgres.Commands/Suspect.TaskPro.Infra.Persistence.Postgres.Commands.csproj", "src/Infra/Persistence/Postgres.Commands/"]
COPY ["src/Infra/Persistence/Postgres.Queries/Suspect.TaskPro.Infra.Persistence.Postgres.Queries.csproj", "src/Infra/Persistence/Postgres.Queries/"]
RUN dotnet restore "./src/Endpoints/Suspect.TaskPro.Endpoints.Api/Suspect.TaskPro.Endpoints.Api.csproj"
COPY . .
WORKDIR "/src/src/Endpoints/Suspect.TaskPro.Endpoints.Api"
RUN dotnet build "./Suspect.TaskPro.Endpoints.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Suspect.TaskPro.Endpoints.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Suspect.TaskPro.Endpoints.Api.dll"]