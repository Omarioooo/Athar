using Microsoft.ML.Data;

namespace AtharPlatform.Services.MachineLearning.Models
{
    /// <summary>
    /// Represents user-campaign interaction for collaborative filtering
    /// </summary>
    public class UserCampaignInteraction
    {
        [LoadColumn(0)]
        public float UserId { get; set; }

        [LoadColumn(1)]
        public float CampaignId { get; set; }

        [LoadColumn(2)]
        public float Rating { get; set; } // Interaction score: donations, views, reactions

        [LoadColumn(3)]
        public string Category { get; set; } = string.Empty;

        [LoadColumn(4)]
        public bool IsCritical { get; set; }
    }

    /// <summary>
    /// Matrix factorization prediction
    /// </summary>
    public class CampaignRatingPrediction
    {
        public float Score { get; set; }
    }
}
