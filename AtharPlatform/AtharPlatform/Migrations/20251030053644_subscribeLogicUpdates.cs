using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AtharPlatform.Migrations
{
    /// <inheritdoc />
    public partial class subscribeLogicUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WalletBalance",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "PaymobTransactionId",
                table: "Donations",
                newName: "TransactionId");

            migrationBuilder.AddColumn<int>(
                name: "FollowId",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MerchantOrderId",
                table: "Donations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentID",
                table: "Donations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Follows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    donornID = table.Column<int>(type: "int", nullable: false),
                    charityID = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Follows_AspNetUsers_charityID",
                        column: x => x.charityID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Follows_AspNetUsers_donornID",
                        column: x => x.donornID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_FollowId",
                table: "Subscriptions",
                column: "FollowId");

            migrationBuilder.CreateIndex(
                name: "IX_Follows_charityID",
                table: "Follows",
                column: "charityID");

            migrationBuilder.CreateIndex(
                name: "IX_Follows_donornID",
                table: "Follows",
                column: "donornID");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Follows_FollowId",
                table: "Subscriptions",
                column: "FollowId",
                principalTable: "Follows",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Follows_FollowId",
                table: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Follows");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_FollowId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "FollowId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "MerchantOrderId",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "PaymentID",
                table: "Donations");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "Donations",
                newName: "PaymobTransactionId");

            migrationBuilder.AddColumn<decimal>(
                name: "WalletBalance",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
