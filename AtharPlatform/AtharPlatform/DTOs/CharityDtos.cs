using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AtharPlatform.Models;

namespace AtharPlatform.Dtos
{
    // Unified card/detail DTO for charities (used across list/detail/search)
    public class CharityCardDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        // Image URL (both manual and scraped charities use file URLs now)
        public string? ImageUrl { get; set; }
        public string? ExternalWebsiteUrl { get; set; }
        public int CampaignsCount { get; set; }
        public IEnumerable<MiniCampaignDto> Campaigns { get; set; } = Array.Empty<MiniCampaignDto>();
    }

    // Public, lightweight card view for lists
    public class CharityListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public int CampaignsCount { get; set; }
    }

    // Detailed view when a user opens a charity page
    public class CharityDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? ExternalWebsiteUrl { get; set; }
        public IEnumerable<MiniCampaignDto> Campaigns { get; set; } = Array.Empty<MiniCampaignDto>();
    }

    public class MiniCampaignDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public double GoalAmount { get; set; }
        public double RaisedAmount { get; set; }
    }

    // Update contract for Charity
    public class CharityUpdateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public byte[]? Image { get; set; }
        // Optional external info updates (for scraped or curated links)
        public string? ImageUrl { get; set; }
        public string? ExternalWebsiteUrl { get; set; }
    }

    // Stats contract for dashboard
    public class CharityStatsDto
    {
        public int TotalCharities { get; set; }
        public int ApprovedCharities { get; set; }
        public int PendingCharities { get; set; }
        public int RejectedCharities { get; set; }

        public int TotalCampaigns { get; set; }
        public int ActiveCampaigns { get; set; }

        public decimal TotalCashDonations { get; set; }
        public int TotalMaterialDonations { get; set; }
        public decimal Last30DaysCash { get; set; }

        // Extensions
        public decimal AverageDonationAmount { get; set; }
        public int DonationsCount { get; set; }
        public IEnumerable<TopCampaignDto> TopCampaigns { get; set; } = Array.Empty<TopCampaignDto>();
    }

    public class TopCampaignDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal TotalDonations { get; set; }
        public int DonationsCount { get; set; }
    }

    public class MaterialDonationDto
    {
        public int Id { get; set; }
        public string DonorFirstName { get; set; } = string.Empty;
        public string DonorLastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string MeasurementUnit { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }

    // Import contract for bulk-scraped charities
    public class CharityImportItemDto
    {
        // These attributes allow binding snake_case fields from scraped JSON (e.g., image_url)
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        // accept base64 image bytes if present (optional); callers can omit
        [JsonPropertyName("image")]
        public byte[]? Image { get; set; }

        // Alternatively, callers can provide a remote image URL
        [JsonPropertyName("image_url")]
        public string? ImageUrl { get; set; }

        [JsonPropertyName("external_website_url")]
        public string? ExternalWebsiteUrl { get; set; }

        // Any external id/source can be carried here for dedupe (optional)
        [JsonPropertyName("external_id")]
        public string? ExternalId { get; set; }
    }
}
