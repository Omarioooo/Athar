using AtharPlatform.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class Campaign
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public byte[]? Image { get; set; }

        public bool isCritical { get; set; } = false;

        [Required]
        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        [Required]
        public int Duration { get; set; }

        // Computed end date from start + duration (not stored)
        [NotMapped]
        public DateTime EndDate => StartDate.AddDays(Duration);

        [Required]
        public double GoalAmount { get; set; }

        [Required]
        public double RaisedAmount { get; set; }

        public bool IsInKindDonation { get; set; } = false;

        public DateTime StartingDate { get; set; } = DateTime.UtcNow;

        public CampaignCategoryEnum Category { get; set; } = CampaignCategoryEnum.Other;
        public CampainStatusEnum Status { get; set; } = CampainStatusEnum.inProgress;


        #region // Scraped Date
        // Scraped source identifier (e.g., megakheir slug)
        public string? ExternalId { get; set; }

        // Full list of scraped supporting charity names (JSON array of strings)
        public string? SupportingCharitiesJson { get; set; }
        #endregion

        [ForeignKey(nameof(Charity))]
        public int CharityID { get; set; }
        public virtual Charity Charity { get; set; } = null!;
        public virtual List<CampaignDonation> CampaignDonations { get; set; } = null!;
        public byte[] ImageUrl { get; internal set; }
    }
}