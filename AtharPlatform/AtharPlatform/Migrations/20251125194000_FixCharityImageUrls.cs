using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AtharPlatform.Migrations
{
    /// <inheritdoc />
    public partial class FixCharityImageUrls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Fix ImageUrl for charity 107 (لنعمرها) - the file exists in wwwroot
            migrationBuilder.Sql(@"
                UPDATE ""Charities"" 
                SET ""ImageUrl"" = '/uploads/charities/charity_107_20251124_233717.jpg'
                WHERE ""Id"" = 107 AND ""ImageUrl"" IS NULL;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert the ImageUrl to null
            migrationBuilder.Sql(@"
                UPDATE ""Charities"" 
                SET ""ImageUrl"" = NULL
                WHERE ""Id"" = 107;
            ");
        }
    }
}
