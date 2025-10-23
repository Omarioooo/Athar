using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public enum CampaignType
    {
        General = 0,
        CriticalCase = 1
    }

    public enum CampaignCategory
    {
        Education = 0,
        Health = 1,
        Orphans = 2,
        Food = 3,
        Shelter = 4,
        Other = 99
    }

    public class Campaign
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
    // Computed end date from start + duration (not stored)
    [NotMapped]
    public DateTime EndDate => StartDate.AddDays(Duration);
        [Required]
        public int Duration { get; set; }
        [Required]
        public double GoalAmount { get; set; }
        [Required]
        public double RaisedAmount { get; set; }

        // Classification
        public CampaignType Type { get; set; } = CampaignType.General;
        public CampaignCategory Category { get; set; } = CampaignCategory.Other;

        // Whether campaign accepts in-kind donations
        public bool AcceptInKindDonations { get; set; } = false;

        public CampainStatusEnum Status { get; set; } = CampainStatusEnum.inProgress;

        public virtual List<CampaignDonation> CampaignDonations { get; set; } = new();
        public virtual List<CharityCampaign> CharityCampaigns { get; set; } = new();
        public virtual List<CampaignContent> CampaignContent { get; set; } = new();

    }
}
