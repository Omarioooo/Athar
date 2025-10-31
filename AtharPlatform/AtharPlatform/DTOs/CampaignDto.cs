using AtharPlatform.Models.Enum;

namespace AtharPlatform.Dtos
{
    public class CampaignDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public double GoalAmount { get; set; }
        public double RaisedAmount { get; set; }
        public CampainStatusEnum Status { get; set; } 
        public CampaignCategoryEnum Category { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCritical { get; set; }

        public string? CharityName { get; set; } 
    }
}