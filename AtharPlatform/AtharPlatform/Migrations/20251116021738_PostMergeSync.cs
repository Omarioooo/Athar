using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AtharPlatform.Migrations
{
    /// <inheritdoc />
    public partial class PostMergeSync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampaignDonations_AspNetUsers_DonorId",
                table: "CampaignDonations");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_AspNetUsers_CharityID",
                table: "Campaigns");

            migrationBuilder.DropForeignKey(
                name: "FK_CharityDonations_AspNetUsers_DonorID",
                table: "CharityDonations");

            migrationBuilder.DropForeignKey(
                name: "FK_CharityDonations_AspNetUsers_charityID",
                table: "CharityDonations");

            migrationBuilder.DropForeignKey(
                name: "FK_CharityExternalInfos_AspNetUsers_CharityId",
                table: "CharityExternalInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_CharityMaterialDonation_AspNetUsers_CharityId",
                table: "CharityMaterialDonation");

            migrationBuilder.DropForeignKey(
                name: "FK_CharityVendorOffers_AspNetUsers_CharityId",
                table: "CharityVendorOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_CharityVolunteers_AspNetUsers_CharityId",
                table: "CharityVolunteers");

            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_AspNetUsers_DonorID",
                table: "Reactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_AspNetUsers_charityID",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_AspNetUsers_donornID",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_SubscriptionTypes_TypeId",
                table: "Subscriptions");

            migrationBuilder.DropTable(
                name: "CampaignContents");

            migrationBuilder.DropTable(
                name: "SubscriptionTypes");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_donornID",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_TypeId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Donations_StripePaymentId",
                table: "Donations");

            migrationBuilder.DropIndex(
                name: "IX_CharityDonations_DonorID",
                table: "CharityDonations");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "IsWebhookProcessed",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "PlatformFee",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "StripePaymentId",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "DonorID",
                table: "CharityDonations");

            migrationBuilder.DropColumn(
                name: "DeactivatedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ImportedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsScraped",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StripCustomerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VerificationDocuments",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "PriceBeAfterDiscount",
                table: "VendorForms",
                newName: "PriceAfterDiscount");

            migrationBuilder.RenameColumn(
                name: "charityID",
                table: "Subscriptions",
                newName: "CharityId");

            migrationBuilder.RenameColumn(
                name: "donornID",
                table: "Subscriptions",
                newName: "DonorId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_charityID",
                table: "Subscriptions",
                newName: "IX_Subscriptions_CharityId");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Notifications",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "Provider",
                table: "Donations",
                newName: "TransactionId");

            migrationBuilder.RenameColumn(
                name: "PaymentReference",
                table: "Donations",
                newName: "Currency");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "VendorForms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Subscriptions",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<string>(
                name: "Frequency",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPaymentDate",
                table: "Subscriptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextPaymentDate",
                table: "Subscriptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "Receivers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReadAt",
                table: "Receivers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReactionDate",
                table: "Reactions",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "DonationStatus",
                table: "Donations",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Donations",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "CharityId",
                table: "Donations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DonorId",
                table: "Donations",
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

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Contents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CampaignId",
                table: "Contents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "DonorId",
                table: "CampaignDonations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Charities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    VerificationDocument = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    IsScraped = table.Column<bool>(type: "bit", nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImportedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DeactivatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Charities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Charities_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Donors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Donors_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Follows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonorId = table.Column<int>(type: "int", nullable: false),
                    CharityId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Follows_Charities_CharityId",
                        column: x => x.CharityId,
                        principalTable: "Charities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Follows_Donors_DonorId",
                        column: x => x.DonorId,
                        principalTable: "Donors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_DonorId_CharityId",
                table: "Subscriptions",
                columns: new[] { "DonorId", "CharityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Donations_CharityId",
                table: "Donations",
                column: "CharityId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_DonorId",
                table: "Donations",
                column: "DonorId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_CampaignId",
                table: "Contents",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Follows_CharityId",
                table: "Follows",
                column: "CharityId");

            migrationBuilder.CreateIndex(
                name: "IX_Follows_DonorId_CharityId",
                table: "Follows",
                columns: new[] { "DonorId", "CharityId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignDonations_Donors_DonorId",
                table: "CampaignDonations",
                column: "DonorId",
                principalTable: "Donors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_Charities_CharityID",
                table: "Campaigns",
                column: "CharityID",
                principalTable: "Charities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CharityDonations_Charities_charityID",
                table: "CharityDonations",
                column: "charityID",
                principalTable: "Charities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CharityExternalInfos_Charities_CharityId",
                table: "CharityExternalInfos",
                column: "CharityId",
                principalTable: "Charities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CharityMaterialDonation_Charities_CharityId",
                table: "CharityMaterialDonation",
                column: "CharityId",
                principalTable: "Charities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CharityVendorOffers_Charities_CharityId",
                table: "CharityVendorOffers",
                column: "CharityId",
                principalTable: "Charities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CharityVolunteers_Charities_CharityId",
                table: "CharityVolunteers",
                column: "CharityId",
                principalTable: "Charities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contents_Campaigns_CampaignId",
                table: "Contents",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_Charities_CharityId",
                table: "Donations",
                column: "CharityId",
                principalTable: "Charities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Donations_Donors_DonorId",
                table: "Donations",
                column: "DonorId",
                principalTable: "Donors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_Donors_DonorID",
                table: "Reactions",
                column: "DonorID",
                principalTable: "Donors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Charities_CharityId",
                table: "Subscriptions",
                column: "CharityId",
                principalTable: "Charities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Donors_DonorId",
                table: "Subscriptions",
                column: "DonorId",
                principalTable: "Donors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampaignDonations_Donors_DonorId",
                table: "CampaignDonations");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_Charities_CharityID",
                table: "Campaigns");

            migrationBuilder.DropForeignKey(
                name: "FK_CharityDonations_Charities_charityID",
                table: "CharityDonations");

            migrationBuilder.DropForeignKey(
                name: "FK_CharityExternalInfos_Charities_CharityId",
                table: "CharityExternalInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_CharityMaterialDonation_Charities_CharityId",
                table: "CharityMaterialDonation");

            migrationBuilder.DropForeignKey(
                name: "FK_CharityVendorOffers_Charities_CharityId",
                table: "CharityVendorOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_CharityVolunteers_Charities_CharityId",
                table: "CharityVolunteers");

            migrationBuilder.DropForeignKey(
                name: "FK_Contents_Campaigns_CampaignId",
                table: "Contents");

            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Charities_CharityId",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_Donations_Donors_DonorId",
                table: "Donations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Donors_DonorID",
                table: "Reactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Charities_CharityId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Donors_DonorId",
                table: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Follows");

            migrationBuilder.DropTable(
                name: "Charities");

            migrationBuilder.DropTable(
                name: "Donors");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_DonorId_CharityId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Donations_CharityId",
                table: "Donations");

            migrationBuilder.DropIndex(
                name: "IX_Donations_DonorId",
                table: "Donations");

            migrationBuilder.DropIndex(
                name: "IX_Contents_CampaignId",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "VendorForms");

            migrationBuilder.DropColumn(
                name: "Frequency",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "LastPaymentDate",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "NextPaymentDate",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "Receivers");

            migrationBuilder.DropColumn(
                name: "ReadAt",
                table: "Receivers");

            migrationBuilder.DropColumn(
                name: "CharityId",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "DonorId",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "MerchantOrderId",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "PaymentID",
                table: "Donations");

            migrationBuilder.DropColumn(
                name: "CampaignId",
                table: "Contents");

            migrationBuilder.RenameColumn(
                name: "PriceAfterDiscount",
                table: "VendorForms",
                newName: "PriceBeAfterDiscount");

            migrationBuilder.RenameColumn(
                name: "CharityId",
                table: "Subscriptions",
                newName: "charityID");

            migrationBuilder.RenameColumn(
                name: "DonorId",
                table: "Subscriptions",
                newName: "donornID");

            migrationBuilder.RenameIndex(
                name: "IX_Subscriptions_CharityId",
                table: "Subscriptions",
                newName: "IX_Subscriptions_charityID");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Notifications",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "Donations",
                newName: "Provider");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "Donations",
                newName: "PaymentReference");

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "Subscriptions",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReactionDate",
                table: "Reactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "Notifications",
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

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Donations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsWebhookProcessed",
                table: "Donations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "PlatformFee",
                table: "Donations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "StripePaymentId",
                table: "Donations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Contents",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "DonorID",
                table: "CharityDonations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "DonorId",
                table: "CampaignDonations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeactivatedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ImportedAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsScraped",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripCustomerId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "VerificationDocuments",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CampaignContents",
                columns: table => new
                {
                    ContentId = table.Column<int>(type: "int", nullable: false),
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignContents", x => x.ContentId);
                    table.ForeignKey(
                        name: "FK_CampaignContents_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignContents_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_donornID",
                table: "Subscriptions",
                column: "donornID");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_TypeId",
                table: "Subscriptions",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_StripePaymentId",
                table: "Donations",
                column: "StripePaymentId",
                unique: true,
                filter: "[StripePaymentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CharityDonations_DonorID",
                table: "CharityDonations",
                column: "DonorID");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignContents_CampaignId",
                table: "CampaignContents",
                column: "CampaignId");

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignDonations_AspNetUsers_DonorId",
                table: "CampaignDonations",
                column: "DonorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_AspNetUsers_CharityID",
                table: "Campaigns",
                column: "CharityID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CharityDonations_AspNetUsers_DonorID",
                table: "CharityDonations",
                column: "DonorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CharityDonations_AspNetUsers_charityID",
                table: "CharityDonations",
                column: "charityID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CharityExternalInfos_AspNetUsers_CharityId",
                table: "CharityExternalInfos",
                column: "CharityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CharityMaterialDonation_AspNetUsers_CharityId",
                table: "CharityMaterialDonation",
                column: "CharityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CharityVendorOffers_AspNetUsers_CharityId",
                table: "CharityVendorOffers",
                column: "CharityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CharityVolunteers_AspNetUsers_CharityId",
                table: "CharityVolunteers",
                column: "CharityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_AspNetUsers_DonorID",
                table: "Reactions",
                column: "DonorID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_AspNetUsers_charityID",
                table: "Subscriptions",
                column: "charityID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_AspNetUsers_donornID",
                table: "Subscriptions",
                column: "donornID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_SubscriptionTypes_TypeId",
                table: "Subscriptions",
                column: "TypeId",
                principalTable: "SubscriptionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
