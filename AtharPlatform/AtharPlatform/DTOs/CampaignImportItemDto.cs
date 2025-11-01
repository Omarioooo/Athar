using System.Text.Json.Serialization;

namespace AtharPlatform.Dtos
{
    public class CampaignImportItemDto
    {
        [JsonPropertyName("title")] public string? Title { get; set; }
        [JsonPropertyName("description")] public string? Description { get; set; }
        // Raw image bytes not supported for campaigns import; we use URL
        [JsonPropertyName("image_url")] public string? ImageUrl { get; set; }

        // If provided, will be stored only in external info in the future; currently unused but accepted for completeness
        [JsonPropertyName("external_website_url")] public string? ExternalWebsiteUrl { get; set; }

        // Names of charities associated with this campaign as scraped (Arabic supported). We'll pick the first that exists.
        [JsonPropertyName("supporting_charities")] public List<string>? SupportingCharities { get; set; }

        // Optional direct charity name override (if present, used first)
        [JsonPropertyName("charity_name")] public string? CharityName { get; set; }

        // Optional numeric fields; if missing, importer will randomize
        [JsonPropertyName("goal_amount")] public double? GoalAmount { get; set; }
        [JsonPropertyName("raised_amount")] public double? RaisedAmount { get; set; }
        [JsonPropertyName("is_critical")] public bool? IsCritical { get; set; }
        [JsonPropertyName("start_date")] public DateTime? StartDate { get; set; }
        [JsonPropertyName("duration_days")] public int? DurationDays { get; set; }

        // Import-time classification; if null, importer will infer from title/description
        [JsonPropertyName("category")] public string? Category { get; set; }

        // Source system identifier if available (for de-duplication)
        [JsonPropertyName("external_id")] public string? ExternalId { get; set; }
    }
}
