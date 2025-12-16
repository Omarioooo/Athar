using AtharPlatform.Models.Enum;

namespace AtharPlatform.Services.MachineLearning.Models
{
    /// <summary>
    /// Aggregated user donation history for feature engineering
    /// </summary>
    public class UserDonationHistory
    {
        public int UserId { get; set; }
        public int TotalDonations { get; set; }
        public decimal TotalAmountDonated { get; set; }
        public List<CampaignCategoryEnum> PreferredCategories { get; set; } = new();
        public List<int> DonatedCampaignIds { get; set; } = new();
        public bool PrefersSmallCharities { get; set; }
        public bool PrefersCriticalCampaigns { get; set; }
        public DateTime LastDonationDate { get; set; }
    }
}
