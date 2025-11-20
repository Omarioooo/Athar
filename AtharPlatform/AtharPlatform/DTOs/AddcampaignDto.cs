using AtharPlatform.Models.Enum;
using AtharPlatform.Validators;
using System.ComponentModel.DataAnnotations;

namespace AtharPlatform.DTOs
{
    public class AddCampaignDto
    {
        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        /// <summary>
        /// Binary image file (for manual uploads). Either Image or ImageUrl must be provided, but not both.
        /// </summary>
        public IFormFile? Image { get; set; }

        /// <summary>
        /// External image URL (for campaigns with hosted images). Either Image or ImageUrl must be provided, but not both.
        /// </summary>
        [ValidImageUrl]
        public string? ImageUrl { get; set; }

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
