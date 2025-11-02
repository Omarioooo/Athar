using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AtharPlatform.Migrations
{
    /// <inheritdoc />
    public partial class dbupdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_AspNetUsers_charityID",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_AspNetUsers_donornID",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Follows_FollowId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_FollowId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "FollowId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Subscriptions");

            migrationBuilder.RenameColumn(
                name: "charityID",
                table: "Subscriptions",
                newName: "CharityID");

            migrationBuilder.RenameColumn(
                name: "donornID",
                table: "Subscriptions",
                newName: "DonorID");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_charityID",
                table: "Subscriptions",
                newName: "IX_Subscriptions_CharityID");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_donornID",
                table: "Subscriptions",
                newName: "IX_Subscriptions_DonorID");

            migrationBuilder.AlterColumn<int>(
                name: "DonationStatus",
                table: "Donations",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_AspNetUsers_CharityID",
                table: "Subscriptions",
                column: "CharityID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_AspNetUsers_DonorID",
                table: "Subscriptions",
                column: "DonorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_AspNetUsers_CharityID",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_AspNetUsers_DonorID",
                table: "Subscriptions");

            migrationBuilder.RenameColumn(
                name: "CharityID",
                table: "Subscriptions",
                newName: "charityID");

            migrationBuilder.RenameColumn(
                name: "DonorID",
                table: "Subscriptions",
                newName: "donornID");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_CharityID",
                table: "Subscriptions",
                newName: "IX_Subscriptions_charityID");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_DonorID",
                table: "Subscriptions",
                newName: "IX_Subscriptions_donornID");

            migrationBuilder.AddColumn<int>(
                name: "FollowId",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Subscriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "DonationStatus",
                table: "Donations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_FollowId",
                table: "Subscriptions",
                column: "FollowId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_AspNetUsers_charityID",
                table: "Subscriptions",
                column: "charityID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_AspNetUsers_donornID",
                table: "Subscriptions",
                column: "donornID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Follows_FollowId",
                table: "Subscriptions",
                column: "FollowId",
                principalTable: "Follows",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
