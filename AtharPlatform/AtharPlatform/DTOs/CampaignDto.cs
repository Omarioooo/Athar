using AtharPlatform.Models.Enum;
using System.Text.Json.Serialization;

namespace AtharPlatform.Dtos
{
    public class CampaignDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string Image { get; set; } 

        public double GoalAmount { get; set; }
        public double RaisedAmount { get; set; }
        public CampainStatusEnum Status { get; set; } 
        public CampaignCategoryEnum Category { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCritical { get; set; }

        // For manually-created campaigns (single owning charity)
        [JsonPropertyName("charity_name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? CharityName { get; set; }

        // For scraped campaigns (multiple supporters). Only populated for scraped items.
        [JsonPropertyName("supporting_charities")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string>? SupportingCharities { get; set; }
    }
}