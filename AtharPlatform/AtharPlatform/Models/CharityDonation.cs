using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class CharityDonation
    {
        [Key,ForeignKey("Donation")]
        public int DonationId { get; set; }
        public Donation Donation { get; set; }


        public DateTime Date { get; set; }


        [ForeignKey("CharityCampaign")]
        public int charityID { get; set; }
        public CharityCampaign CharityCampaign { get; set; }


        [ForeignKey("user")]
        public string DonorID { get; set; }
        public User user { get; set; }


    }
}
