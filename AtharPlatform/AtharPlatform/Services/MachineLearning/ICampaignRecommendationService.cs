using AtharPlatform.Services.MachineLearning.Models;

namespace AtharPlatform.Services.MachineLearning
{
    /// <summary>
    /// Service for AI-powered campaign recommendations
    /// </summary>
    public interface ICampaignRecommendationService
    {
        /// <summary>
        /// Get hybrid recommendations combining content-based and collaborative filtering
        /// with boosting for new charities and low-donation campaigns
        /// </summary>
        Task<List<RecommendationResult>> GetHybridRecommendationsAsync(int? userId = null, int topN = 10);

        /// <summary>
        /// Train or retrain the ML models
        /// </summary>
        Task TrainModelsAsync();

        /// <summary>
        /// Check if models are trained and ready
        /// </summary>
        bool IsModelReady();
    }
}
