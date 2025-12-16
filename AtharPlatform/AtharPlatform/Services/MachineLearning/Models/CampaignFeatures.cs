using Microsoft.ML.Data;

namespace AtharPlatform.Services.MachineLearning.Models
{
    /// <summary>
    /// Input data for campaign text features (content-based filtering)
    /// </summary>
    public class CampaignFeatures
    {
        [LoadColumn(0)]
        public int CampaignId { get; set; }

        [LoadColumn(1)]
        public string Title { get; set; } = string.Empty;

        [LoadColumn(2)]
        public string Description { get; set; } = string.Empty;

        [LoadColumn(3)]
        public string Category { get; set; } = string.Empty;

        [LoadColumn(4)]
        public bool IsCritical { get; set; }

        [LoadColumn(5)]
        public float GoalAmount { get; set; }

        [LoadColumn(6)]
        public float ProgressPercentage { get; set; }
    }

    /// <summary>
    /// Output prediction for campaign similarity
    /// </summary>
    public class CampaignSimilarityPrediction
    {
        [VectorType(100)] // Adjust based on feature size
        public float[] Features { get; set; } = Array.Empty<float>();
    }
}
