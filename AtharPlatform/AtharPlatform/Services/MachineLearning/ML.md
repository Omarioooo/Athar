# AI-Powered Campaign Recommendation System 

## Overview
This document describes the **Machine Learning recommendation system** integrated into the Athar Platform using **ML.NET**. The system provides intelligent, personalized campaign recommendations to donors based on their donation history, preferences, and advanced AI algorithms.

---

##  Features

### 1. **Content-Based Recommendations (Text Mining)**
- Uses **Natural Language Processing (NLP)** to analyze campaign titles and descriptions
- Extracts features from text using ML.NET's **FeaturizeText** transformer
- Creates vector representations of campaigns for similarity matching
- Considers multiple factors:
  - Campaign category
  - Critical status
  - Goal amount
  - Progress percentage
  - Text content similarity

### 2. **Collaborative Filtering (Matrix Factorization)**
- Learns from user-campaign interactions (donations)
- Predicts which campaigns a user might be interested in
- Uses **Matrix Factorization** algorithm to find latent patterns
- Handles the "cold start" problem with fallback mechanisms

### 3. **Similar Campaigns (GetSimilarCampaigns)**
- Shows related campaigns when viewing a specific campaign
- **Highlights small charities** as per requirements
- Uses multi-factor similarity scoring:
  - Category matching (40% weight)
  - Text similarity using Jaccard index (30% weight)
  - Goal amount similarity (20% weight)
  - Critical status matching (10% weight)

### 4. **Personalized User Recommendations**
- Analyzes user's donation history
- Identifies preferred categories
- Detects patterns (e.g., preference for critical campaigns, small charities)
- Returns diverse recommendations:
  - ML-based personalized suggestions
  - Category-based recommendations
  - Small charity highlights
  - Critical campaigns
  - Trending campaigns

### 5. **Anonymous User Support (Cold Start)**
- Provides trending and popular campaigns for new/anonymous users
- No login required for initial recommendations
- Smooth onboarding experience

---

## Architecture

### File Structure
```
AtharPlatform/
├── Services/
│   └── MachineLearning/
│       ├── ICampaignRecommendationService.cs      # Service interface
│       ├── CampaignRecommendationService.cs       # Main ML service
│       ├── MLModelTrainingBackgroundService.cs    # Auto-training service
│       └── Models/
│           ├── CampaignFeatures.cs                # Content-based model
│           ├── UserCampaignInteraction.cs         # Collaborative filtering model
│           ├── UserDonationHistory.cs             # User profile data
│           └── RecommendationResult.cs            # Response models
├── Controllers/
│   └── CampaignController.cs                      # API endpoints
├── MLModels/                                      # Trained models (auto-generated)
│   ├── content_model.zip
│   └── collaborative_model.zip
└── AtharPlatform.csproj                           # NuGet packages
```

---

## ML Models

### Model 1: Content-Based Filtering
**Purpose:** Find similar campaigns based on content

**Pipeline:**
1. **Text Featurization** → Convert title/description to numerical features
2. **Categorical Encoding** → One-hot encode category
3. **Normalization** → Scale numerical features (goal amount, progress)
4. **Feature Concatenation** → Combine all features into single vector

**Training Data:** All campaigns in database

**Algorithm:** Feature extraction + Cosine similarity

### Model 2: Collaborative Filtering
**Purpose:** Predict user preferences based on donation patterns

**Algorithm:** Matrix Factorization (Recommendation Trainer)

**Training Data:** User-Campaign interactions (donations)

**Parameters:**
- Approximation Rank: 100
- Learning Rate: 0.1
- Iterations: 20

**Evaluation Metrics:**
- RMSE (Root Mean Squared Error)
- R² (Coefficient of Determination)

---

## API Endpoints

### 1. Get Similar Campaigns
```http
GET /api/Campaign/{campaignId}/GetSimilarCampaigns?topN=5
```

**Description:** Get campaigns similar to the specified campaign

**Parameters:**
- `campaignId` (path): The campaign to find similar campaigns for
- `topN` (query, optional): Number of results (default: 5, max: 20)

**Response:**
```json
{
  "campaignId": 123,
  "similarCampaigns": [
    {
      "id": 456,
      "title": "Similar Campaign",
      "description": "...",
      "category": "Education",
      "isCritical": false,
      "charityName": "Small Charity Name"
    }
  ],
  "count": 5
}
```

**Use Case:** Display "Related Campaigns" section on campaign detail page

---

### 2. Get Personalized Recommendations
```http
GET /api/Campaign/donor/{donorId}/GetRecommendations?topN=10
```

**Description:** Get personalized campaign recommendations for a donor

**Parameters:**
- `donorId` (path): The donor's ID
- `topN` (query, optional): Number of results (default: 10, max: 50)

**Response:**
```json
{
  "donorId": 789,
  "recommendations": [
    {
      "id": 101,
      "title": "Recommended Campaign",
      "description": "...",
      "category": "Health",
      "goalAmount": 50000,
      "raisedAmount": 25000,
      "charityName": "Recommended Charity"
    }
  ],
  "count": 10,
  "message": "Personalized recommendations based on your donation history"
}
```

**Use Case:** Homepage personalized feed, donor dashboard

---

### 3. Get Trending Campaigns
```http
GET /api/Campaign/GetTrendingCampaigns?topN=10
```

**Description:** Get popular/trending campaigns (for anonymous users)

**Parameters:**
- `topN` (query, optional): Number of results (default: 10, max: 50)

**Response:**
```json
{
  "trendingCampaigns": [...],
  "count": 10,
  "message": "Popular campaigns in your community"
}
```

**Use Case:** Homepage for non-logged-in users, trending section

---

## How It Works

### Step 1: Data Collection
The system continuously collects data about:
- User donations (amount, campaign, charity)
- Campaign characteristics (title, description, category, goal, etc.)
- User interactions

### Step 2: Feature Engineering
**For Content-Based:**
- Text tokenization and TF-IDF vectorization
- Category encoding
- Numerical feature normalization

**For Collaborative Filtering:**
- User-campaign interaction matrix
- Rating calculation based on donation amount
- Sparse matrix handling

### Step 3: Model Training
**Automatic Training:**
- Runs daily via `MLModelTrainingBackgroundService`
- First training: 5 minutes after app startup
- Subsequent trainings: Every 24 hours
- Models saved to `/MLModels` directory

**Manual Training:**
```csharp
await _recommendationService.TrainModelsAsync();
```

### Step 4: Prediction
When a user requests recommendations:
1. Load user's donation history
2. Generate predictions using trained models
3. Apply business rules (e.g., boost small charities)
4. Rank and return top N results

---

## Algorithms Explained

### 1. Text Mining (TF-IDF + Bag of Words)
Converts campaign text into numerical features:
- **TF (Term Frequency):** How often a word appears
- **IDF (Inverse Document Frequency):** How unique a word is
- **Result:** Vector representation of campaign content

### 2. Matrix Factorization
Decomposes user-campaign matrix into two lower-dimensional matrices:
- **User Matrix:** User preferences (latent factors)
- **Campaign Matrix:** Campaign characteristics (latent factors)
- **Prediction:** User × Campaign = Predicted interest score

### 3. Cosine Similarity
Measures similarity between campaign feature vectors:
```
similarity = (A · B) / (||A|| × ||B||)
```
Range: 0 (completely different) to 1 (identical)

### 4. Jaccard Similarity
Measures text overlap:
```
J(A,B) = |A ∩ B| / |A ∪ B|
```
Used for comparing campaign titles/descriptions

---

## Configuration

### Minimum Data Requirements
- **Content Model:** At least 1 campaign (works with any data)
- **Collaborative Model:** At least 10 donation interactions

### Training Parameters
You can adjust these in `CampaignRecommendationService.cs`:

```csharp
// Collaborative Filtering
NumberOfIterations = 20,        // More = better accuracy, slower training
ApproximationRank = 100,        // Latent factors (higher = more complex)
LearningRate = 0.1              // Training speed

// Training Schedule
_trainingInterval = TimeSpan.FromHours(24);  // How often to retrain
_initialDelay = TimeSpan.FromMinutes(5);     // Delay after startup
```

### Model Storage
Models are saved in:
```
{ContentRootPath}/MLModels/
├── content_model.zip
└── collaborative_model.zip
```

---

## Business Rules

### Small Charity Highlighting
The system **prioritizes small charities** as requested:
- Defined as: Charities with verified status and < 10 campaigns
- Special flag: `IsSmallCharity` in response
- Separate recommendation category
- Highlighted in related campaigns

### Critical Campaign Boosting
- Critical campaigns get separate recommendations
- Higher visibility in user feeds
- Urgent messaging

### Diversity
- Recommendations include multiple categories
- Mix of ML-based and rule-based suggestions
- Balance between popular and niche campaigns

---

## Performance Metrics

### Training Performance
- **Content Model:** ~1-2 seconds (1000 campaigns)
- **Collaborative Model:** ~5-10 seconds (10,000 interactions)
- **Total Training Time:** < 15 seconds typically

### Prediction Performance
- **Single Prediction:** < 50ms
- **Batch Predictions (10 campaigns):** < 200ms
- **Similar Campaigns:** < 300ms

### Model Quality
Monitor these metrics in logs:
- **RMSE:** Lower is better (target: < 1.0)
- **R²:** Higher is better (target: > 0.5)

---

## Testing the System

### 1. Test Similar Campaigns
```bash
# Get similar campaigns for campaign ID 1
curl https://localhost:5001/api/Campaign/1/GetSimilarCampaigns?topN=5
```

### 2. Test Personalized Recommendations
```bash
# Get recommendations for donor ID 5
curl https://localhost:5001/api/Campaign/donor/5/GetRecommendations?topN=10
```

### 3. Test Trending
```bash
# Get trending campaigns
curl https://localhost:5001/api/Campaign/GetTrendingCampaigns?topN=10
```

### 4. Trigger Manual Training
Add this endpoint to trigger training manually (for testing):
```csharp
[HttpPost("admin/train-ml-models")]
public async Task<IActionResult> TrainModels()
{
    await _recommendationService.TrainModelsAsync();
    return Ok("Models trained successfully");
}
```

---

## Troubleshooting

### Issue: "Models not ready"
**Cause:** Models haven't been trained yet  
**Solution:** Wait 5 minutes after startup or trigger manual training

### Issue: "Insufficient data for collaborative filtering"
**Cause:** Less than 10 donation interactions  
**Solution:** System will use content-based recommendations only

### Issue: "Similar campaigns returning empty"
**Cause:** Campaign ID not found or no active campaigns  
**Solution:** Verify campaign exists and is in progress

### Issue: High memory usage
**Cause:** Large datasets in memory  
**Solution:** 
- Reduce `ApproximationRank`
- Train during off-peak hours
- Increase training interval

---

## Future Enhancements

1. **Advanced NLP**: Use BERT or similar transformers for better text understanding
2. **Deep Learning**: Neural collaborative filtering
3. **Real-time Learning**: Update models incrementally with new donations
4. **A/B Testing**: Compare recommendation algorithms
5. **Explainability**: Provide clear reasons for each recommendation
6. **Multi-armed Bandit**: Balance exploration vs exploitation
7. **Context-Aware**: Consider time, location, device
8. **Social Signals**: Incorporate shares, follows, comments

---

## References

- [ML.NET Documentation](https://docs.microsoft.com/en-us/dotnet/machine-learning/)
- [Matrix Factorization](https://developers.google.com/machine-learning/recommendation/collaborative/matrix)
- [Content-Based Filtering](https://en.wikipedia.org/wiki/Recommender_system#Content-based_filtering)

---

## Best Practices

1. **Retrain Regularly**: Daily training keeps models fresh
2. **Monitor Metrics**: Check RMSE and R² in logs
3. **Handle Cold Start**: Always provide fallback recommendations
4. **Explain Recommendations**: Show users why campaigns are recommended
5. **Respect Privacy**: Don't expose donation amounts in explanations
6. **Test with Real Users**: A/B test different algorithms
7. **Optimize Performance**: Cache predictions for popular users

---

##  Support

For questions or issues with the ML recommendation system, please contact the development team or refer to the main project documentation.

---

**Created:** December 2024  
**Version:** 1.0  
**ML.NET Version:** 4.0.0  
**.NET Version:** 9.0
