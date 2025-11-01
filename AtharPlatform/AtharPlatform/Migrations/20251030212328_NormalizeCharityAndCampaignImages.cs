using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AtharPlatform.Migrations
{
    /// <inheritdoc />
    public partial class NormalizeCharityAndCampaignImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Idempotent create of CharityExternalInfos if not exists
            migrationBuilder.Sql(@"
IF OBJECT_ID(N'[dbo].[CharityExternalInfos]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[CharityExternalInfos](
        [CharityId] [int] NOT NULL,
        [ImageUrl] [nvarchar](max) NULL,
        [ExternalWebsiteUrl] [nvarchar](max) NULL,
        [MegaKheirUrl] [nvarchar](max) NULL,
        CONSTRAINT [PK_CharityExternalInfos] PRIMARY KEY CLUSTERED ([CharityId] ASC),
        CONSTRAINT [FK_CharityExternalInfos_AspNetUsers_CharityId] FOREIGN KEY([CharityId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END
");

            // Campaigns: add ImageUrl if missing
            migrationBuilder.Sql(@"
IF COL_LENGTH('Campaigns','ImageUrl') IS NULL
BEGIN
    ALTER TABLE [dbo].[Campaigns] ADD [ImageUrl] nvarchar(max) NOT NULL CONSTRAINT DF_Campaigns_ImageUrl DEFAULT('');
END
");
            // Campaigns: drop Image varbinary if exists
            migrationBuilder.Sql(@"
IF COL_LENGTH('Campaigns','Image') IS NOT NULL
BEGIN
    ALTER TABLE [dbo].[Campaigns] DROP COLUMN [Image];
END
");

            // Drop old Charity link columns from AspNetUsers if they exist (moved to CharityExternalInfos)
            migrationBuilder.Sql(@"
IF COL_LENGTH('AspNetUsers','ImageUrl') IS NOT NULL
BEGIN
    ALTER TABLE [dbo].[AspNetUsers] DROP COLUMN [ImageUrl];
END
IF COL_LENGTH('AspNetUsers','ExternalWebsiteUrl') IS NOT NULL
BEGIN
    ALTER TABLE [dbo].[AspNetUsers] DROP COLUMN [ExternalWebsiteUrl];
END
IF COL_LENGTH('AspNetUsers','MegaKheirUrl') IS NOT NULL
BEGIN
    ALTER TABLE [dbo].[AspNetUsers] DROP COLUMN [MegaKheirUrl];
END
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Recreate old Charity link columns if missing
            migrationBuilder.Sql(@"
IF COL_LENGTH('AspNetUsers','ImageUrl') IS NULL
BEGIN
    ALTER TABLE [dbo].[AspNetUsers] ADD [ImageUrl] nvarchar(max) NULL;
END
IF COL_LENGTH('AspNetUsers','ExternalWebsiteUrl') IS NULL
BEGIN
    ALTER TABLE [dbo].[AspNetUsers] ADD [ExternalWebsiteUrl] nvarchar(max) NULL;
END
IF COL_LENGTH('AspNetUsers','MegaKheirUrl') IS NULL
BEGIN
    ALTER TABLE [dbo].[AspNetUsers] ADD [MegaKheirUrl] nvarchar(max) NULL;
END
");

            // Re-add Image if missing and drop ImageUrl
            migrationBuilder.Sql(@"
IF COL_LENGTH('Campaigns','Image') IS NULL
BEGIN
    ALTER TABLE [dbo].[Campaigns] ADD [Image] varbinary(max) NOT NULL CONSTRAINT DF_Campaigns_Image DEFAULT(0x);
END
IF COL_LENGTH('Campaigns','ImageUrl') IS NOT NULL
BEGIN
    ALTER TABLE [dbo].[Campaigns] DROP COLUMN [ImageUrl];
END
");

            // Drop CharityExternalInfos if exists
            migrationBuilder.Sql(@"
IF OBJECT_ID(N'[dbo].[CharityExternalInfos]', N'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[CharityExternalInfos];
END
");
        }
    }
}
