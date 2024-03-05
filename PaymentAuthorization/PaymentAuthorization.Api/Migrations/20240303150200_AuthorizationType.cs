using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaymentAuthorization.Api.Migrations
{
    /// <inheritdoc />
    public partial class AuthorizationType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuthorizationTypeEnum",
                table: "PaymentAuthorizationRequests",
                newName: "AuthorizationType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuthorizationType",
                table: "PaymentAuthorizationRequests",
                newName: "AuthorizationTypeEnum");
        }
    }
}
