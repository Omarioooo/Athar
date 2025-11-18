using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AtharPlatform.Migrations
{
    /// <inheritdoc />
    public partial class EditPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampaignDonations_Campaigns_CampaignId",
                table: "CampaignDonations");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignDonations_Donations_DonationId",
                table: "CampaignDonations");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignDonations_Donors_DonorId",
                table: "CampaignDonations");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_Charities_CharityID",
                table: "Campaigns");

            migrationBuilder.DropForeignKey(
                name: "FK_Charities_AspNetUsers_Id",
                table: "Charities");

            migrationBuilder.DropForeignKey(
                name: "FK_CharityDonations_Charities_charityID",
                table: "CharityDonations");

            migrationBuilder.DropForeignKey(
                name: "FK_CharityDonations_Donations_DonationId",
                table: "CharityDonations");

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignDonations_Campaigns_CampaignId",
                table: "CampaignDonations",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignDonations_Donations_DonationId",
                table: "CampaignDonations",
                column: "DonationId",
                principalTable: "Donations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignDonations_Donors_DonorId",
                table: "CampaignDonations",
                column: "DonorId",
                principalTable: "Donors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_Charities_CharityID",
                table: "Campaigns",
                column: "CharityID",
                principalTable: "Charities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Charities_AspNetUsers_Id",
                table: "Charities",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_CharityDonations_Charities_charityID",
                table: "CharityDonations",
                column: "charityID",
                principalTable: "Charities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CharityDonations_Donations_DonationId",
                table: "CharityDonations",
                column: "DonationId",
                principalTable: "Donations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampaignDonations_Campaigns_CampaignId",
                table: "CampaignDonations");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignDonations_Donations_DonationId",
                table: "CampaignDonations");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignDonations_Donors_DonorId",
                table: "CampaignDonations");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_Charities_CharityID",
                table: "Campaigns");

            migrationBuilder.DropForeignKey(
                name: "FK_Charities_AspNetUsers_Id",
                table: "Charities");

            migrationBuilder.DropForeignKey(
                name: "FK_CharityDonations_Charities_charityID",
                table: "CharityDonations");

            migrationBuilder.DropForeignKey(
                name: "FK_CharityDonations_Donations_DonationId",
                table: "CharityDonations");

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignDonations_Campaigns_CampaignId",
                table: "CampaignDonations",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignDonations_Donations_DonationId",
                table: "CampaignDonations",
                column: "DonationId",
                principalTable: "Donations",
                principalColumn: "Id");

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
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Charities_AspNetUsers_Id",
                table: "Charities",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CharityDonations_Charities_charityID",
                table: "CharityDonations",
                column: "charityID",
                principalTable: "Charities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CharityDonations_Donations_DonationId",
                table: "CharityDonations",
                column: "DonationId",
                principalTable: "Donations",
                principalColumn: "Id");
        }
    }
}
