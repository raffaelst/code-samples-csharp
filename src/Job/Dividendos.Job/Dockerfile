FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-bionic AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-bionic AS build
WORKDIR /src
COPY ["Job/Dividendos.Job/Dividendos.Job.csproj", "Job/Dividendos.Job/"]
COPY ["Core/CrossCutting/Dividendos.CrossCutting.IoC/Dividendos.CrossCutting.IoC.csproj", "Core/CrossCutting/Dividendos.CrossCutting.IoC/"]
COPY ["App/ExternalService/Dividendos.B3/Dividendos.B3.csproj", "App/ExternalService/Dividendos.B3/"]
COPY ["App/Interface/Dividendos.B3.Interface/Dividendos.B3.Interface.csproj", "App/Interface/Dividendos.B3.Interface/"]
COPY ["App/Interface/Dividendos.Finance.Interface/Dividendos.Finance.Interface.csproj", "App/Interface/Dividendos.Finance.Interface/"]
COPY ["Core/Interface/Dividendos.Repository.Interface/Dividendos.Repository.Interface.csproj", "Core/Interface/Dividendos.Repository.Interface/"]
COPY ["Core/Dividendos.Entities/Dividendos.Entity.csproj", "Core/Dividendos.Entities/"]
COPY ["Core/CrossCutting/Dividendos.CrossCutting.Identity/Dividendos.CrossCutting.Identity.csproj", "Core/CrossCutting/Dividendos.CrossCutting.Identity/"]
COPY ["Core/CrossCutting/Dividendos.CrossCutting.Config/Dividendos.CrossCutting.Config.csproj", "Core/CrossCutting/Dividendos.CrossCutting.Config/"]
COPY ["Core/Dividendos.Services/Dividendos.Service.csproj", "Core/Dividendos.Services/"]
COPY ["Core/CrossCutting/Dividendos.CrossCutting.Mapper/Dividendos.CrossCutting.Mapper.csproj", "Core/CrossCutting/Dividendos.CrossCutting.Mapper/"]
COPY ["API/Dividendos.API.Model/Dividendos.API.Model.csproj", "API/Dividendos.API.Model/"]
COPY ["Core/Interface/Dividendos.Service.Interfaces/Dividendos.Service.Interface.csproj", "Core/Interface/Dividendos.Service.Interfaces/"]
COPY ["App/ExternalService/Dividendos.Finance/Dividendos.Finance.csproj", "App/ExternalService/Dividendos.Finance/"]
COPY ["App/Interface/Dividendos.Application.Interface/Dividendos.Application.Interface.csproj", "App/Interface/Dividendos.Application.Interface/"]
COPY ["Core/Dividendos.Repository/Dividendos.Repository.csproj", "Core/Dividendos.Repository/"]
COPY ["App/ExternalService/Dividendos.Bacen/Dividendos.Bacen.csproj", "App/ExternalService/Dividendos.Bacen/"]
COPY ["App/Interface/Dividendos.Bacen.Interface/Dividendos.Bacen.Interface.csproj", "App/Interface/Dividendos.Bacen.Interface/"]
COPY ["App/Dividendos.Application/Dividendos.Application.csproj", "App/Dividendos.Application/"]
RUN dotnet restore "Job/Dividendos.Job/Dividendos.Job.csproj"
COPY . .
WORKDIR "/src/Job/Dividendos.Job"
RUN dotnet build "Dividendos.Job.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Dividendos.Job.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Dividendos.Job.dll"]