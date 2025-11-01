using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AtharPlatform.Migrations
{
    /// <inheritdoc />
    public partial class paymentSchemaUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymobWalletId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "PaymentServiceId",
                table: "Donations",
                newName: "PaymobTransactionId");

            migrationBuilder.AddColumn<int>(
                name: "CharityId",
                table: "Donations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WalletBalance",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Donations_CharityId",
                table: "Donations",
                column: "CharityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_AspNetUsers_CharityId",
                table: "Donations",
                column: "CharityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Donations_AspNetUsers_CharityId",
                table: "Donations");

            migrationBuilder.DropIndex(
                name: "IX_Donations_CharityId",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "CharityId",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "WalletBalance",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "PaymobTransactionId",
                table: "Donations",
                newName: "PaymentServiceId");

            migrationBuilder.AddColumn<string>(
                name: "PaymobWalletId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
