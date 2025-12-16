using AtharPlatform.Models;
using AtharPlatform.Models.Enum;
using AtharPlatform.Repositories;
using AtharPlatform.Services.MachineLearning.Models;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using System.Text;

namespace AtharPlatform.Services.MachineLearning
{
    /// <summary>
    /// AI-powered campaign recommendation service using ML.NET
    /// Implements both collaborative filtering and content-based recommendations
    /// </summary>
    public class CampaignRecommendationService : ICampaignRecommendationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CampaignRecommendationService> _logger;
        private readonly MLContext _mlContext;
        private readonly string _modelBasePath;
        
        private ITransformer? _contentBasedModel;
        private ITransformer? _collaborativeModel;
        private PredictionEngine<CampaignFeatures, CampaignSimilarityPrediction>? _contentPredictionEngine;
        private PredictionEngine<UserCampaignInteraction, CampaignRatingPrediction>? _collaborativePredictionEngine;
        
        private readonly SemaphoreSlim _trainingLock = new(1, 1);
        private DateTime? _lastTrainingTime;
        private const int MinimumDataPoints = 10; // Minimum interactions needed for training

        public CampaignRecommendationService(
            IServiceProvider serviceProvider,
            ILogger<CampaignRecommendationService> logger,
            IWebHostEnvironment environment)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _mlContext = new MLContext(seed: 1);
            _modelBasePath = Path.Combine(environment.ContentRootPath, "MLModels");
            
            // Ensure model directory exists
            Directory.CreateDirectory(_modelBasePath);
            
            // Try to load existing models
            LoadModelsIfExist();
        }

        public bool IsModelReady()
        {
            return _contentBasedModel != null && _collaborativeModel != null;
        }

        #region Public Recommendation Methods

        /// <summary>
        /// Hybrid recommendation system combining content-based and collaborative filtering
        /// with boosting for new charities and low-donation campaigns
        /// </summary>
        public async Task<List<RecommendationResult>> GetHybridRecommendationsAsync(int? userId = null, int topN = 10)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                
                // Get all active campaigns
                var activeCampaigns = await unitOfWork.Campaigns
                    .GetAllInProgressAsync(includeCharity: true);

                if (!activeCampaigns.Any())
                {
                    return new List<RecommendationResult>();
                }

                var scoredCampaigns = new List<(Campaign campaign, float score)>();

                // Get user history if userId provided
                UserDonationHistory? userHistory = null;
                if (userId.HasValue)
                {
                    userHistory = await BuildUserDonationHistoryAsync(userId.Value, unitOfWork);
                    // Filter out already donated campaigns
                    activeCampaigns = activeCampaigns
                        .Where(c => !userHistory.DonatedCampaignIds.Contains(c.Id))
                        .ToList();
                }

                foreach (var campaign in activeCampaigns)
                {
                    float hybridScore = 0f;

                    // 1. Content-Based Score (40% weight) - Text similarity, category match
                    float contentScore = CalculateContentScore(campaign, userHistory);
                    hybridScore += contentScore * 0.4f;

                    // 2. Collaborative Filtering Score (30% weight) - User behavior patterns
                    if (userId.HasValue && IsModelReady() && userHistory?.TotalDonations >= 3)
                    {
                        float collaborativeScore = PredictUserCampaignRating(userId.Value, campaign.Id);
                        hybridScore += collaborativeScore * 0.3f;
                    }
                    else
                    {
                        // Fallback: popularity score for new users
                        float popularityScore = (float)(campaign.RaisedAmount / Math.Max(campaign.GoalAmount, 1));
                        hybridScore += popularityScore * 0.3f;
                    }

                    // 3. Campaign Health Score (20% weight) - Critical status, progress
                    float healthScore = CalculateCampaignHealthScore(campaign);
                    hybridScore += healthScore * 0.2f;

                    // 4. Diversity Score (10% weight) - Category variety
                    float diversityScore = userHistory != null 
                        ? CalculateDiversityScore(campaign, userHistory)
                        : 0.5f;
                    hybridScore += diversityScore * 0.1f;

                    // **BOOSTING for New Charities and Low-Donation Campaigns**
                    hybridScore = ApplyBoostFactors(hybridScore, campaign);

                    scoredCampaigns.Add((campaign, hybridScore));
                }

                // Sort by final hybrid score and return top N
                var results = scoredCampaigns
                    .OrderByDescending(x => x.score)
                    .Take(topN > 0 ? topN : scoredCampaigns.Count) // Allow "all" by passing 0 or negative
                    .Select((x, index) => new RecommendationResult
                    {
                        CampaignId = x.campaign.Id,
                        Title = x.campaign.Title,
                        Score = x.score,
                        IsSmallCharity = IsSmallCharity(x.campaign.Charity),
                        RecommendationReason = BuildHybridRecommendationReason(x.campaign, userHistory)
                    })
                    .ToList();

                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating hybrid recommendations");
                return new List<RecommendationResult>();
            }
        }

        // Legacy methods - NOT USED (replaced by GetHybridRecommendationsAsync)
        /*
        public async Task<CampaignRecommendationResponse> GetRecommendationsForUserAsync(int userId, int topN = 10)
        {
            // DEPRECATED: Use GetHybridRecommendationsAsync instead
            throw new NotImplementedException("Use GetHybridRecommendationsAsync instead");
        }

        public async Task<List<RecommendationResult>> GetSimilarCampaignsAsync(int campaignId, int topN = 5)
        {
            // DEPRECATED: Use GetHybridRecommendationsAsync instead
            throw new NotImplementedException("Use GetHybridRecommendationsAsync instead");
        }

        public async Task<CampaignRecommendationResponse> GetAnonymousRecommendationsAsync()
        {
            // DEPRECATED: Use GetHybridRecommendationsAsync with userId=null instead
            throw new NotImplementedException("Use GetHybridRecommendationsAsync with userId=null instead");
        }
        */

        #endregion

        #region ML Training

        public async Task TrainModelsAsync()
        {
            await _trainingLock.WaitAsync();
            try
            {
                _logger.LogInformation("Starting ML model training...");

                using var scope = _serviceProvider.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                // Train Content-Based Model
                await TrainContentBasedModelAsync(unitOfWork);

                // Train Collaborative Filtering Model
                await TrainCollaborativeFilteringModelAsync(unitOfWork);

                _lastTrainingTime = DateTime.UtcNow;
                _logger.LogInformation("ML model training completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during ML model training");
                throw;
            }
            finally
            {
                _trainingLock.Release();
            }
        }

        private async Task TrainContentBasedModelAsync(IUnitOfWork unitOfWork)
        {
            try
            {
                var campaigns = await unitOfWork.Campaigns.GetAllAsync(includeCharity: true);
                
                if (!campaigns.Any())
                {
                    _logger.LogWarning("No campaigns available for content-based training");
                    return;
                }

                var campaignFeatures = campaigns.Select(c => new CampaignFeatures
                {
                    CampaignId = c.Id,
                    Title = c.Title ?? string.Empty,
                    Description = c.Description ?? string.Empty,
                    Category = c.Category.ToString(),
                    IsCritical = c.isCritical,
                    GoalAmount = (float)c.GoalAmount,
                    ProgressPercentage = (float)(c.RaisedAmount / Math.Max(c.GoalAmount, 1) * 100)
                }).ToList();

                var dataView = _mlContext.Data.LoadFromEnumerable(campaignFeatures);

                // Build text featurization pipeline
                var pipeline = _mlContext.Transforms.Text.FeaturizeText(
                        outputColumnName: "TitleFeatures",
                        inputColumnName: nameof(CampaignFeatures.Title))
                    .Append(_mlContext.Transforms.Text.FeaturizeText(
                        outputColumnName: "DescriptionFeatures",
                        inputColumnName: nameof(CampaignFeatures.Description)))
                    .Append(_mlContext.Transforms.Categorical.OneHotEncoding(
                        outputColumnName: "CategoryEncoded",
                        inputColumnName: nameof(CampaignFeatures.Category)))
                    .Append(_mlContext.Transforms.Conversion.ConvertType(
                        outputColumnName: "IsCriticalFloat",
                        inputColumnName: nameof(CampaignFeatures.IsCritical),
                        outputKind: DataKind.Single))
                    .Append(_mlContext.Transforms.NormalizeMinMax(
                        outputColumnName: "GoalAmountNormalized",
                        inputColumnName: nameof(CampaignFeatures.GoalAmount)))
                    .Append(_mlContext.Transforms.NormalizeMinMax(
                        outputColumnName: "ProgressNormalized",
                        inputColumnName: nameof(CampaignFeatures.ProgressPercentage)))
                    .Append(_mlContext.Transforms.Concatenate(
                        "Features",
                        "TitleFeatures",
                        "DescriptionFeatures",
                        "CategoryEncoded",
                        "IsCriticalFloat",
                        "GoalAmountNormalized",
                        "ProgressNormalized"));

                _contentBasedModel = pipeline.Fit(dataView);
                
                // Save model
                var contentModelPath = Path.Combine(_modelBasePath, "content_model.zip");
                _mlContext.Model.Save(_contentBasedModel, dataView.Schema, contentModelPath);

                // Create prediction engine
                _contentPredictionEngine = _mlContext.Model
                    .CreatePredictionEngine<CampaignFeatures, CampaignSimilarityPrediction>(_contentBasedModel);

                _logger.LogInformation("Content-based model trained successfully with {Count} campaigns", campaigns.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error training content-based model");
            }
        }

        private async Task TrainCollaborativeFilteringModelAsync(IUnitOfWork unitOfWork)
        {
            try
            {
                var interactions = await BuildUserCampaignInteractionsAsync(unitOfWork);
                
                if (interactions.Count < MinimumDataPoints)
                {
                    _logger.LogWarning("Insufficient data for collaborative filtering. Need at least {Min} interactions, got {Count}",
                        MinimumDataPoints, interactions.Count);
                    return;
                }

                var dataView = _mlContext.Data.LoadFromEnumerable(interactions);

                // Split data for training and validation
                var split = _mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);

                // Matrix Factorization for collaborative filtering
                var options = new MatrixFactorizationTrainer.Options
                {
                    MatrixColumnIndexColumnName = nameof(UserCampaignInteraction.UserId),
                    MatrixRowIndexColumnName = nameof(UserCampaignInteraction.CampaignId),
                    LabelColumnName = nameof(UserCampaignInteraction.Rating),
                    NumberOfIterations = 20,
                    ApproximationRank = 100,
                    LearningRate = 0.1
                };

                var trainer = _mlContext.Recommendation().Trainers.MatrixFactorization(options);
                _collaborativeModel = trainer.Fit(split.TrainSet);

                // Evaluate
                var predictions = _collaborativeModel.Transform(split.TestSet);
                var metrics = _mlContext.Regression.Evaluate(predictions, 
                    labelColumnName: nameof(UserCampaignInteraction.Rating));

                _logger.LogInformation("Collaborative model trained. RMSE: {RMSE}, R2: {R2}",
                    metrics.RootMeanSquaredError, metrics.RSquared);

                // Save model
                var collabModelPath = Path.Combine(_modelBasePath, "collaborative_model.zip");
                _mlContext.Model.Save(_collaborativeModel, dataView.Schema, collabModelPath);

                // Create prediction engine
                _collaborativePredictionEngine = _mlContext.Model
                    .CreatePredictionEngine<UserCampaignInteraction, CampaignRatingPrediction>(_collaborativeModel);

                _logger.LogInformation("Collaborative filtering model trained with {Count} interactions", interactions.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error training collaborative filtering model");
            }
        }

        #endregion

        #region Helper Methods

        private async Task<UserDonationHistory> BuildUserDonationHistoryAsync(int userId, IUnitOfWork unitOfWork)
        {
            var donations = await unitOfWork.PaymentDonations.GetDonationsByDonorIdAsync(userId);
            
            var history = new UserDonationHistory
            {
                UserId = userId,
                TotalDonations = donations.Count,
                TotalAmountDonated = donations.Sum(d => d.TotalAmount),
                LastDonationDate = donations.Any() ? donations.Max(d => d.CreatedAt ?? DateTime.MinValue) : DateTime.MinValue
            };

            // Extract campaign IDs and categories
            foreach (var donation in donations)
            {
                foreach (var campaignDonation in donation.CampaignDonations)
                {
                    history.DonatedCampaignIds.Add(campaignDonation.CampaignId);
                    
                    var campaign = await unitOfWork.Campaigns.GetAsync(campaignDonation.CampaignId);
                    if (campaign != null)
                    {
                        if (!history.PreferredCategories.Contains(campaign.Category))
                        {
                            history.PreferredCategories.Add(campaign.Category);
                        }
                        
                        if (campaign.isCritical)
                        {
                            history.PrefersCriticalCampaigns = true;
                        }
                    }
                }
            }

            return history;
        }

        private async Task<List<UserCampaignInteraction>> BuildUserCampaignInteractionsAsync(IUnitOfWork unitOfWork)
        {
            var interactions = new List<UserCampaignInteraction>();
            var donations = await unitOfWork.PaymentDonations.GetAllAsync();

            foreach (var donation in donations)
            {
                foreach (var campaignDonation in donation.CampaignDonations)
                {
                    var campaign = await unitOfWork.Campaigns.GetAsync(campaignDonation.CampaignId);
                    if (campaign == null) continue;

                    // Rating based on donation amount (normalized)
                    float rating = Math.Min((float)donation.TotalAmount / 100.0f, 5.0f);

                    interactions.Add(new UserCampaignInteraction
                    {
                        UserId = donation.DonorId,
                        CampaignId = campaignDonation.CampaignId,
                        Rating = rating,
                        Category = campaign.Category.ToString(),
                        IsCritical = campaign.isCritical
                    });
                }
            }

            return interactions;
        }

        private async Task<List<RecommendationResult>> GetMLBasedRecommendationsAsync(
            int userId, List<Campaign> candidateCampaigns, int topN)
        {
            if (_collaborativePredictionEngine == null)
                return new List<RecommendationResult>();

            var predictions = new List<(Campaign campaign, float score)>();

            foreach (var campaign in candidateCampaigns)
            {
                try
                {
                    var prediction = _collaborativePredictionEngine.Predict(new UserCampaignInteraction
                    {
                        UserId = userId,
                        CampaignId = campaign.Id
                    });

                    predictions.Add((campaign, prediction.Score));
                }
                catch
                {
                    // Skip if prediction fails
                    continue;
                }
            }

            return predictions
                .OrderByDescending(p => p.score)
                .Take(topN)
                .Select(p => new RecommendationResult
                {
                    CampaignId = p.campaign.Id,
                    Title = p.campaign.Title,
                    Score = p.score,
                    IsSmallCharity = IsSmallCharity(p.campaign.Charity),
                    RecommendationReason = "Based on your donation history"
                })
                .ToList();
        }

        private List<RecommendationResult> GetCategoryBasedRecommendations(
            List<Campaign> campaigns, UserDonationHistory userHistory, int topN)
        {
            var recommendations = campaigns
                .Where(c => userHistory.PreferredCategories.Contains(c.Category))
                .OrderByDescending(c => c.Date)
                .Take(topN)
                .Select(c => new RecommendationResult
                {
                    CampaignId = c.Id,
                    Title = c.Title,
                    Score = 0.8f,
                    IsSmallCharity = IsSmallCharity(c.Charity),
                    RecommendationReason = $"Based on your interest in {c.Category}"
                })
                .ToList();

            return recommendations;
        }

        private async Task<List<RecommendationResult>> GetSmallCharityCampaignsAsync(
            List<Campaign> campaigns, UserDonationHistory? userHistory, int topN)
        {
            // Small charities: fewer than 5 campaigns or less than 50,000 EGP total raised
            var smallCharityCampaigns = new List<RecommendationResult>();

            foreach (var campaign in campaigns)
            {
                if (IsSmallCharity(campaign.Charity))
                {
                    smallCharityCampaigns.Add(new RecommendationResult
                    {
                        CampaignId = campaign.Id,
                        Title = campaign.Title,
                        Score = 1.0f,
                        IsSmallCharity = true,
                        RecommendationReason = "Support small charities making a difference"
                    });
                }
            }

            return smallCharityCampaigns
                .OrderByDescending(r => r.Score)
                .Take(topN)
                .ToList();
        }

        private List<RecommendationResult> GetCriticalCampaigns(
            List<Campaign> campaigns, UserDonationHistory? userHistory, int topN)
        {
            return campaigns
                .Where(c => c.isCritical)
                .OrderByDescending(c => c.Date)
                .Take(topN)
                .Select(c => new RecommendationResult
                {
                    CampaignId = c.Id,
                    Title = c.Title,
                    Score = 1.0f,
                    IsSmallCharity = IsSmallCharity(c.Charity),
                    RecommendationReason = "Urgent campaign needs your help"
                })
                .ToList();
        }

        private async Task<List<RecommendationResult>> GetTrendingCampaignsAsync(
            List<Campaign> campaigns, int topN)
        {
            // Trending: high recent donation activity
            return campaigns
                .OrderByDescending(c => c.RaisedAmount)
                .ThenByDescending(c => c.Date)
                .Take(topN)
                .Select(c => new RecommendationResult
                {
                    CampaignId = c.Id,
                    Title = c.Title,
                    Score = (float)(c.RaisedAmount / Math.Max(c.GoalAmount, 1)),
                    IsSmallCharity = IsSmallCharity(c.Charity),
                    RecommendationReason = "Trending campaign in your community"
                })
                .ToList();
        }

        private float CalculateCampaignSimilarity(Campaign target, Campaign candidate)
        {
            float score = 0f;

            // Category match (40%)
            if (target.Category == candidate.Category)
                score += 0.4f;

            // Critical status match (10%)
            if (target.isCritical == candidate.isCritical)
                score += 0.1f;

            // Goal amount similarity (20%)
            var goalRatio = Math.Min(target.GoalAmount, candidate.GoalAmount) / 
                           Math.Max(target.GoalAmount, candidate.GoalAmount);
            score += (float)(goalRatio * 0.2);

            // Text similarity using simple word overlap (30%)
            score += CalculateTextSimilarity(target.Title + " " + target.Description,
                                            candidate.Title + " " + candidate.Description) * 0.3f;

            return score;
        }

        private float CalculateTextSimilarity(string text1, string text2)
        {
            var words1 = text1.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToHashSet();
            var words2 = text2.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToHashSet();

            if (!words1.Any() || !words2.Any()) return 0f;

            var intersection = words1.Intersect(words2).Count();
            var union = words1.Union(words2).Count();

            return (float)intersection / union; // Jaccard similarity
        }

        private string BuildSimilarityReason(Campaign target, Campaign similar)
        {
            var reasons = new List<string>();

            if (target.Category == similar.Category)
                reasons.Add($"same category ({similar.Category})");

            if (target.isCritical && similar.isCritical)
                reasons.Add("also critical");

            return reasons.Any() 
                ? $"Similar because: {string.Join(", ", reasons)}"
                : "Similar campaign";
        }

        private bool IsSmallCharity(Charity charity)
        {
            if (charity == null) return false;
            
            // Define small charity criteria
            // You can customize this based on your business rules
            // For now, consider a charity small if it has less than 10 campaigns
            return (charity.Campaigns?.Count ?? 0) < 10;
        }

        private void LoadModelsIfExist()
        {
            try
            {
                var contentModelPath = Path.Combine(_modelBasePath, "content_model.zip");
                var collabModelPath = Path.Combine(_modelBasePath, "collaborative_model.zip");

                if (File.Exists(contentModelPath))
                {
                    _contentBasedModel = _mlContext.Model.Load(contentModelPath, out var contentSchema);
                    _contentPredictionEngine = _mlContext.Model
                        .CreatePredictionEngine<CampaignFeatures, CampaignSimilarityPrediction>(_contentBasedModel);
                    _logger.LogInformation("Loaded existing content-based model");
                }

                if (File.Exists(collabModelPath))
                {
                    _collaborativeModel = _mlContext.Model.Load(collabModelPath, out var collabSchema);
                    _collaborativePredictionEngine = _mlContext.Model
                        .CreatePredictionEngine<UserCampaignInteraction, CampaignRatingPrediction>(_collaborativeModel);
                    _logger.LogInformation("Loaded existing collaborative filtering model");
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not load existing models, will need to train");
            }
        }

        #region Hybrid Recommendation Helper Methods

        /// <summary>
        /// Calculate content-based score using category, text, and goal similarity
        /// </summary>
        private float CalculateContentScore(Campaign campaign, UserDonationHistory? userHistory)
        {
            float score = 0.5f; // Base score

            if (userHistory != null && userHistory.PreferredCategories.Any())
            {
                // Boost if campaign matches user's preferred categories
                if (userHistory.PreferredCategories.Contains(campaign.Category))
                {
                    score += 0.3f;
                }
            }

            // Recent campaigns get a small boost
            var daysSinceCreation = (DateTime.Now - campaign.Date).TotalDays;
            if (daysSinceCreation <= 30)
            {
                score += 0.2f * (1 - (float)(daysSinceCreation / 30));
            }

            return Math.Min(score, 1.0f);
        }

        /// <summary>
        /// Calculate campaign health score based on progress and urgency
        /// </summary>
        private float CalculateCampaignHealthScore(Campaign campaign)
        {
            float score = 0f;

            // Progress score (how close to goal)
            var progressRatio = campaign.RaisedAmount / Math.Max(campaign.GoalAmount, 1);
            
            // Boost campaigns that are 20-80% funded (sweet spot for donations)
            if (progressRatio >= 0.2 && progressRatio <= 0.8)
            {
                score += 0.5f;
            }
            else if (progressRatio < 0.2)
            {
                score += 0.3f; // Still needs help
            }

            // Critical campaigns get boost
            if (campaign.isCritical)
            {
                score += 0.5f;
            }

            return Math.Min(score, 1.0f);
        }

        /// <summary>
        /// Calculate diversity score - encourage variety in recommendations
        /// </summary>
        private float CalculateDiversityScore(Campaign campaign, UserDonationHistory userHistory)
        {
            // If user has no history in this category, give diversity boost
            if (!userHistory.PreferredCategories.Contains(campaign.Category))
            {
                return 0.7f; // Encourage exploration
            }
            return 0.3f; // Already familiar category
        }

        /// <summary>
        /// Apply boost factors for new charities and low-donation campaigns
        /// </summary>
        private float ApplyBoostFactors(float baseScore, Campaign campaign)
        {
            float boostedScore = baseScore;

            // **BOOST 1: New Charity (moderate boost: +15%)
            if (IsNewCharity(campaign.Charity))
            {
                boostedScore *= 1.15f;
                _logger.LogDebug("Boosted campaign {CampaignId} for new charity: +15%", campaign.Id);
            }

            // **BOOST 2: Low Donations (moderate boost: +20% if <20% funded)
            var fundingRatio = campaign.RaisedAmount / Math.Max(campaign.GoalAmount, 1);
            if (fundingRatio < 0.2) // Less than 20% funded
            {
                boostedScore *= 1.20f;
                _logger.LogDebug("Boosted campaign {CampaignId} for low donations: +20%", campaign.Id);
            }
            else if (fundingRatio < 0.5) // Less than 50% funded
            {
                boostedScore *= 1.10f;
                _logger.LogDebug("Boosted campaign {CampaignId} for moderate donations: +10%", campaign.Id);
            }

            // **BOOST 3: Small Charity (small boost: +10%)
            if (IsSmallCharity(campaign.Charity))
            {
                boostedScore *= 1.10f;
                _logger.LogDebug("Boosted campaign {CampaignId} for small charity: +10%", campaign.Id);
            }

            // Cap the boost to prevent over-promotion (max 1.5x boost)
            return Math.Min(boostedScore, baseScore * 1.5f);
        }

        /// <summary>
        /// Check if charity is new (created within last 6 months)
        /// </summary>
        private bool IsNewCharity(Charity charity)
        {
            if (charity == null) return false;
            
            // Consider new if less than 3 campaigns (no creation date available)
            return (charity.Campaigns?.Count ?? 0) < 3;
        }

        /// <summary>
        /// Predict user-campaign rating using collaborative filtering model
        /// </summary>
        private float PredictUserCampaignRating(int userId, int campaignId)
        {
            if (_collaborativePredictionEngine == null)
                return 0.5f;

            try
            {
                var prediction = _collaborativePredictionEngine.Predict(new UserCampaignInteraction
                {
                    UserId = userId,
                    CampaignId = campaignId
                });

                return Math.Clamp(prediction.Score / 5.0f, 0f, 1f); // Normalize to 0-1
            }
            catch
            {
                return 0.5f;
            }
        }

        /// <summary>
        /// Build human-readable recommendation reason
        /// </summary>
        private string BuildHybridRecommendationReason(Campaign campaign, UserDonationHistory? userHistory)
        {
            var reasons = new List<string>();

            if (userHistory != null && userHistory.PreferredCategories.Contains(campaign.Category))
            {
                reasons.Add($"matches your interest in {campaign.Category}");
            }

            if (IsNewCharity(campaign.Charity))
            {
                reasons.Add("new charity");
            }

            if (IsSmallCharity(campaign.Charity))
            {
                reasons.Add("small charity making impact");
            }

            var fundingRatio = campaign.RaisedAmount / Math.Max(campaign.GoalAmount, 1);
            if (fundingRatio < 0.2)
            {
                reasons.Add("needs initial support");
            }

            if (campaign.isCritical)
            {
                reasons.Add("urgent campaign");
            }

            return reasons.Any() 
                ? string.Join(", ", reasons)
                : "recommended for you";
        }

        #endregion

        #endregion
    }
}
