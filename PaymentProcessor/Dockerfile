# Imagen base
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copiar csproj y restaurar dependencias
COPY PaymentProcessorCaller/*.csproj ./PaymentProcessorCaller/
COPY PaymentProcessor/*.csproj ./PaymentProcessor/
RUN dotnet restore ./PaymentProcessorCaller/PaymentProcessorCaller.csproj
RUN dotnet restore ./PaymentProcessor/PaymentProcessor.csproj

# Copiar todo y compilar
COPY PaymentProcessorCaller/ ./PaymentProcessorCaller/
COPY PaymentProcessor/ ./PaymentProcessor/
WORKDIR /app/PaymentProcessorCaller
RUN dotnet publish -c Release -o out

# Generar imagen de runtime
FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
COPY --from=build-env /app/PaymentProcessorCaller/out .
ENTRYPOINT ["dotnet", "PaymentProcessorCaller.dll"]