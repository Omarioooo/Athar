using AtharPlatform.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace AtharPlatform.DTOs
{
    public class AddCampaignDto
    {
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }


        [Required]
        public byte[] Image { get; set; }

        public bool IsCritical { get; set; } = false;

        public DateTime? StartDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int Duration { get; set; }

        [Required]
        public double GoalAmount { get; set; }

        public bool IsInKindDonation { get; set; } = false;

        [Required]
        public CampaignCategoryEnum Category { get; set; }

        [Required]
        public int CharityID { get; set; }
    }
}
