using System.Text.Json.Serialization;

namespace AtharPlatform.Dtos
{
    public class CharityCountDto
    {
        [JsonPropertyName("charity_id")] public int CharityId { get; set; }
        [JsonPropertyName("charity_name")] public string CharityName { get; set; } = string.Empty;
        [JsonPropertyName("campaigns_count")] public int CampaignsCount { get; set; }
    }

    public class CharityCampaignsSummaryDto : CharityCountDto
    {
        [JsonPropertyName("campaigns")] public IEnumerable<MiniCampaignDto> Campaigns { get; set; } = Array.Empty<MiniCampaignDto>();
    }
}
