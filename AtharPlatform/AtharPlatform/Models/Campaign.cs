using System.ComponentModel.DataAnnotations;

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
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public double GoalAmount { get; set; }
        [Required]
        public double RaisedAmount { get; set; }

        public CampainStatusEnum Status { get; set; } = CampainStatusEnum.inProgress;

        public List<CampaignDonation> CampaignDonations { get; set; }
        public List<CharityCampaign> CharityCampaigns { get; set; }
        public List<CampaignContent> CampaignContent { get; set; }

    }
}
