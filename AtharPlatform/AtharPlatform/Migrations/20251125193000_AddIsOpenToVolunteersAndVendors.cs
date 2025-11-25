using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AtharPlatform.Migrations
{
    /// <inheritdoc />
    public partial class AddIsOpenToVolunteersAndVendors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add IsOpen column to CharityVolunteers if it doesn't exist
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                                   WHERE table_name='CharityVolunteers' AND column_name='IsOpen') THEN
                        ALTER TABLE ""CharityVolunteers"" ADD COLUMN ""IsOpen"" boolean NOT NULL DEFAULT true;
                    END IF;
                END $$;
            ");

            // Add IsOpen column to CharityVendorOffers if it doesn't exist
            migrationBuilder.Sql(@"
                DO $$ 
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns 
                                   WHERE table_name='CharityVendorOffers' AND column_name='IsOpen') THEN
                        ALTER TABLE ""CharityVendorOffers"" ADD COLUMN ""IsOpen"" boolean NOT NULL DEFAULT true;
                    END IF;
                END $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOpen",
                table: "CharityVolunteers");

            migrationBuilder.DropColumn(
                name: "IsOpen",
                table: "CharityVendorOffers");
        }
    }
}
