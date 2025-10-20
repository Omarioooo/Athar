using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class CharityDonation
    {
        [ForeignKey("Donation")]
        public int DonationId { get; set; }
        public Donation Donation { get; set; }


        public DateTime Date { get; set; }


        [ForeignKey("CharityCampaign")]
        public int charityID { get; set; }
        public CharityCampaign CharityCampaign { get; set; }



    }
}
