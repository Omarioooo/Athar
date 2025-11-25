using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AtharPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToCharity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Image column already exists, only add ImageUrl
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Charities",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Charities");
        }
    }
}
