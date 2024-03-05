export PATH="$PATH:/root/.dotnet/tools"

dotnet ef database update -c AuthorizationDbContext --project /app/PaymentAuthorization.Api.csproj

