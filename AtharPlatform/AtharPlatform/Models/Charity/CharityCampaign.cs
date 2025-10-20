using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class CharityCampaign
    {
        public DateTime StartDate { get; set; }


        [ForeignKey("Charity")]
        public int charityID { get; set; }
        public Charity Charity { get; set; }


        [ForeignKey("Campaign")]
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }


        public List<CharityDonation> CharityDonations { get; set; }
        public List<CampaignContent> campaignContents { get; set; }
    }
}
