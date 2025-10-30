using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AtharPlatform.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelForExternalInfoAndImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Make operations idempotent since earlier migration applied them.
            migrationBuilder.Sql(@"
IF COL_LENGTH('Campaigns','Image') IS NOT NULL
BEGIN
    DECLARE @c nvarchar(128);
    SELECT @c = [d].[name] FROM [sys].[default_constraints] d
    INNER JOIN [sys].[columns] c ON d.parent_column_id = c.column_id AND d.parent_object_id = c.object_id
    WHERE d.parent_object_id = OBJECT_ID(N'[Campaigns]') AND c.[name] = N'Image';
    IF @c IS NOT NULL EXEC(N'ALTER TABLE [Campaigns] DROP CONSTRAINT ['+@c+']');
    ALTER TABLE [Campaigns] DROP COLUMN [Image];
END

IF COL_LENGTH('AspNetUsers','ExternalWebsiteUrl') IS NOT NULL
BEGIN
    ALTER TABLE [AspNetUsers] DROP COLUMN [ExternalWebsiteUrl];
END
IF COL_LENGTH('AspNetUsers','ImageUrl') IS NOT NULL
BEGIN
    ALTER TABLE [AspNetUsers] DROP COLUMN [ImageUrl];
END
IF COL_LENGTH('AspNetUsers','MegaKheirUrl') IS NOT NULL
BEGIN
    ALTER TABLE [AspNetUsers] DROP COLUMN [MegaKheirUrl];
END

IF COL_LENGTH('Campaigns','ImageUrl') IS NULL
BEGIN
    ALTER TABLE [Campaigns] ADD [ImageUrl] nvarchar(max) NOT NULL CONSTRAINT DF_Campaigns_ImageUrl2 DEFAULT('');
END

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF OBJECT_ID(N'[dbo].[CharityExternalInfos]', N'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[CharityExternalInfos];
END

IF COL_LENGTH('Campaigns','ImageUrl') IS NOT NULL
BEGIN
    ALTER TABLE [Campaigns] DROP COLUMN [ImageUrl];
END
IF COL_LENGTH('Campaigns','Image') IS NULL
BEGIN
    ALTER TABLE [Campaigns] ADD [Image] varbinary(max) NOT NULL CONSTRAINT DF_Campaigns_Image2 DEFAULT(0x);
END

IF COL_LENGTH('AspNetUsers','ExternalWebsiteUrl') IS NULL
BEGIN
    ALTER TABLE [AspNetUsers] ADD [ExternalWebsiteUrl] nvarchar(max) NULL;
END
IF COL_LENGTH('AspNetUsers','ImageUrl') IS NULL
BEGIN
    ALTER TABLE [AspNetUsers] ADD [ImageUrl] nvarchar(max) NULL;
END
IF COL_LENGTH('AspNetUsers','MegaKheirUrl') IS NULL
BEGIN
    ALTER TABLE [AspNetUsers] ADD [MegaKheirUrl] nvarchar(max) NULL;
END
");
        }
    }
}
