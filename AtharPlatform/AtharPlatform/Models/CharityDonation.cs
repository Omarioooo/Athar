using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class CharityDonation
    {
        [Key]
        [ForeignKey(nameof(Donation))]
        public int DonationId { get; set; }
        public virtual Donation Donation { get; set; } = new();


        [ForeignKey(nameof(Charity))]
        public int charityID { get; set; }
        public virtual Charity Charity { get; set; } = new();
    }
}
