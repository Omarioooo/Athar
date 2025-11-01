using System.Text.Json.Serialization;

namespace AtharPlatform.Dtos
{
    // Read-only view model that matches the scraped shape (snake_case)
    public class CampaignScrapedDto
    {
        [JsonPropertyName("title")] public string Title { get; set; }
        [JsonPropertyName("description")] public string Description { get; set; }
        [JsonPropertyName("image_url")] public string ImageUrl { get; set; }
        [JsonPropertyName("supporting_charities")] public IEnumerable<string> SupportingCharities { get; set; } = Array.Empty<string>();
        [JsonPropertyName("goal_amount")] public double GoalAmount { get; set; }
        [JsonPropertyName("raised_amount")] public double RaisedAmount { get; set; }
        [JsonPropertyName("is_critical")] public bool IsCritical { get; set; }
        [JsonPropertyName("start_date")] public string StartDate { get; set; } // yyyy-MM-dd
        [JsonPropertyName("duration_days")] public int DurationDays { get; set; }
        [JsonPropertyName("category")] public string Category { get; set; }
        [JsonPropertyName("external_id")] public string? ExternalId { get; set; }
    }
}
