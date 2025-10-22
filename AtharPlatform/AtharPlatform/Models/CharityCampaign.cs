using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class CharityCampaign
    {

        [ForeignKey(nameof(Charity))]
        public int CharityID { get; set; }
        public virtual Charity Charity { get; set; } = new();


        [Key, ForeignKey(nameof(Campaign))]
        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; } = new();


        public virtual List<CharityDonation> CharityDonations { get; set; } = new();

    }
}
