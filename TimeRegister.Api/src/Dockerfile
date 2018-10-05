FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5000

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["TimeRegister.Api/TimeRegister.Api.csproj", "TimeRegister.Api/"]
RUN dotnet restore "TimeRegister.Api/TimeRegister.Api.csproj"
COPY . .
WORKDIR "/src/TimeRegister.Api"
RUN dotnet build "TimeRegister.Api.csproj" -c Release -o /app
FROM build AS publish
RUN dotnet publish "TimeRegister.Api.csproj" -c Release -o /app
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TimeRegister.Api.dll"]