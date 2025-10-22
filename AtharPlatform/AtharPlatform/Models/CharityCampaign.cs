using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class CharityCampaign
    {

        [ForeignKey(nameof(Charity))]
        public int CharityID { get; set; }
        public Charity Charity { get; set; }


        [Key, ForeignKey(nameof(Campaign))]
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }


        public List<CharityDonation> CharityDonations { get; set; }

    }
}
