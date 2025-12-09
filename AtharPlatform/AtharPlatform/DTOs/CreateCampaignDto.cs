using AtharPlatform.Models.Enum;

namespace AtharPlatform.DTOs
{
    public class CreateCampaignDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Duration { get; set; }
        public double GoalAmount { get; set; }

        public CampaignCategoryEnum Category { get; set; }

        public IFormFile ImageFile { get; set; } = null!;
    }
}
