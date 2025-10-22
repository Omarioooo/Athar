using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class CharityDonation
    {
        [Key, ForeignKey(nameof(Donation))]
        public int DonationId { get; set; }
        public Donation Donation { get; set; }


        [ForeignKey(nameof(Charity))]
        public int charityID { get; set; }
        public Charity Charity { get; set; }


        [ForeignKey(nameof(Donor))]
        public string DonorID { get; set; }
        public Donor Donor { get; set; }
    }
}
