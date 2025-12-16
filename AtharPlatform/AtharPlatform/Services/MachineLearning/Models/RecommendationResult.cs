namespace AtharPlatform.Services.MachineLearning.Models
{
    /// <summary>
    /// Campaign recommendation with score
    /// </summary>
    public class RecommendationResult
    {
        public int CampaignId { get; set; }
        public string Title { get; set; } = string.Empty;
        public float Score { get; set; }
        public string RecommendationReason { get; set; } = string.Empty;
        public bool IsSmallCharity { get; set; }
    }

    /// <summary>
    /// Detailed recommendation response
    /// </summary>
    public class CampaignRecommendationResponse
    {
        public List<RecommendationResult> PersonalizedRecommendations { get; set; } = new();
        public List<RecommendationResult> TrendingCampaigns { get; set; } = new();
        public List<RecommendationResult> SmallCharityCampaigns { get; set; } = new();
        public List<RecommendationResult> CriticalCampaigns { get; set; } = new();
    }
}
