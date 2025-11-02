using Microsoft.EntityFrameworkCore.Migrations;

namespace AtharPlatform.Migrations
{
    public partial class AddCampaignScrapedFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "Campaigns",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupportingCharitiesJson",
                table: "Campaigns",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "SupportingCharitiesJson",
                table: "Campaigns");
        }
    }
}
