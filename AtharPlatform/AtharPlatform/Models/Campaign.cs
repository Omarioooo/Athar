using System.ComponentModel.DataAnnotations;

namespace AtharPlatform.Models
{
    public class Campaign
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        public DateTime Duration { get; set; }
        public decimal GoalAmount {get; set; }

        public decimal RaisedAmount { get; set; } = 0;

        public bool Status { get; set; }

        public List<CampaignDonation> CampaignDonations { get; set; }
        public List<CharityCampaign> CharityCampaigns { get; set; }
        public List<CampaignContent> CampaignContent { get; set; }


    }
}
