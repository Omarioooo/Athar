using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AtharPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddCampaignImageValidation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Campaign_ImageSource",
                table: "Campaigns",
                sql: "([Image] IS NOT NULL AND [ImageUrl] IS NULL) OR ([Image] IS NULL AND [ImageUrl] IS NOT NULL)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Campaign_ImageSource",
                table: "Campaigns");
        }
    }
}
