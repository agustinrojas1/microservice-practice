# Imagen base
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copiar csproj y restaurar dependencias
COPY *.csproj ./
RUN dotnet restore

# Copiar todo y compilar
COPY . ./
RUN dotnet publish -c Release -o out

# Generar imagen de runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
COPY --from=build-env /app/PaymentAuthorization.Api.csproj .

# # Instalar dotnet-ef
# RUN dotnet tool install --global dotnet-ef --version 7.0.16

# Asegurarse de que el PATH está configurado correctamente
ENV PATH="${PATH}:/root/.dotnet/tools"

ENV ASPNETCORE_URLS=http://+:7212

#COPY ./entrypoint.sh ./
#RUN chmod +x ./entrypoint.sh

ENTRYPOINT ["dotnet", "PaymentAuthorization.Api.dll"]