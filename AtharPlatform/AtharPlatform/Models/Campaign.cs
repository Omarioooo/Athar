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
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public byte[] Image { get; set; }

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

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public CampaignCategoryEnum Category { get; set; } = CampaignCategoryEnum.Other;
        public CampainStatusEnum Status { get; set; } = CampainStatusEnum.inProgress;


        [ForeignKey(nameof(Charity))]
        public int CharityID { get; set; }
        public virtual Charity Charity { get; set; } = new();
        public virtual List<CampaignDonation> CampaignDonations { get; set; } = new();
        public virtual List<Campaign> Campaigns { get; set; } = new();
        public virtual List<CampaignContent> CampaignContent { get; set; } = new();
    }
}