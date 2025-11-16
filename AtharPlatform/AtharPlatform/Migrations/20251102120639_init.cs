using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AtharPlatform.Migrations
{
    /// <inheritdoc />
    public partial class init2_20251102120639 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                        // Duplicate initial migration detected post-merge. Intentionally left empty to avoid re-creating existing tables.
                    table.ForeignKey(
                        name: "FK_Donors_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        protected override void Up(MigrationBuilder migrationBuilder)
                        {
                            // Neutralized duplicate migration; intentionally left empty.
                        }
                        principalTable: "Donations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReactionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DonorID = table.Column<int>(type: "int", nullable: false),
                    ContentID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reactions_Contents_ContentID",
                        column: x => x.ContentID,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Reactions_Donors_DonorID",
                        column: x => x.DonorID,
                        principalTable: "Donors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignDonations_CampaignId",
                table: "CampaignDonations",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignDonations_DonorId",
                table: "CampaignDonations",
                column: "DonorId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_CharityID",
                table: "Campaigns",
                column: "CharityID");

            migrationBuilder.CreateIndex(
                name: "IX_CharityDonations_charityID",
                table: "CharityDonations",
                column: "charityID");

            migrationBuilder.CreateIndex(
                name: "IX_CharityMaterialDonation_CharityId",
                table: "CharityMaterialDonation",
                column: "CharityId");

            migrationBuilder.CreateIndex(
                name: "IX_CharityVendorOffers_CharityId",
                table: "CharityVendorOffers",
                column: "CharityId");

            migrationBuilder.CreateIndex(
                name: "IX_CharityVolunteers_CharityId",
                table: "CharityVolunteers",
                column: "CharityId");

            migrationBuilder.CreateIndex(
                name: "IX_Contents_CampaignId",
                table: "Contents",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_CharityId",
                table: "Donations",
                column: "CharityId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_DonorId",
                table: "Donations",
                column: "DonorId");

            migrationBuilder.CreateIndex(
                name: "IX_Follows_CharityId",
                table: "Follows",
                column: "CharityId");

            migrationBuilder.CreateIndex(
                name: "IX_Follows_DonorId_CharityId",
                table: "Follows",
                columns: new[] { "DonorId", "CharityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaterialDonations_MaterialDonationId",
                table: "MaterialDonations",
                column: "MaterialDonationId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_ContentID",
                table: "Reactions",
                column: "ContentID");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_DonorID",
                table: "Reactions",
                column: "DonorID");

            migrationBuilder.CreateIndex(
                name: "IX_Receivers_ReceiverId",
                table: "Receivers",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Sender_SenderId",
                table: "Sender",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_CharityId",
                table: "Subscriptions",
                column: "CharityId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_DonorId_CharityId",
                table: "Subscriptions",
                columns: new[] { "DonorId", "CharityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VendorForms_CharityVendorOfferId",
                table: "VendorForms",
                column: "CharityVendorOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_VolunteerForm_CharityVolunteerId",
                table: "VolunteerForm",
                column: "CharityVolunteerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Down intentionally left empty since Up did nothing.
        }
    }
}
