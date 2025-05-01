using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestMobiBuy.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNameColumnCustomerAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_customer_addresses_customers_CustomerId",
                schema: "app",
                table: "customer_addresses");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                schema: "app",
                table: "customer_addresses",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                schema: "app",
                table: "customer_addresses",
                newName: "customer_id");

            migrationBuilder.RenameIndex(
                name: "IX_customer_addresses_CustomerId",
                schema: "app",
                table: "customer_addresses",
                newName: "IX_customer_addresses_customer_id");

            migrationBuilder.AddForeignKey(
                name: "FK_customer_addresses_customers_customer_id",
                schema: "app",
                table: "customer_addresses",
                column: "customer_id",
                principalSchema: "app",
                principalTable: "customers",
                principalColumn: "customer_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_customer_addresses_customers_customer_id",
                schema: "app",
                table: "customer_addresses");

            migrationBuilder.RenameColumn(
                name: "is_active",
                schema: "app",
                table: "customer_addresses",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "customer_id",
                schema: "app",
                table: "customer_addresses",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_customer_addresses_customer_id",
                schema: "app",
                table: "customer_addresses",
                newName: "IX_customer_addresses_CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_customer_addresses_customers_CustomerId",
                schema: "app",
                table: "customer_addresses",
                column: "CustomerId",
                principalSchema: "app",
                principalTable: "customers",
                principalColumn: "customer_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
