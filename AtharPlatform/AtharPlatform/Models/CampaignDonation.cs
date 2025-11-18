using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class CampaignDonation
    {
        [Key]
        [ForeignKey(nameof(Donation))]
        public int DonationId { get; set; }
        public virtual Donation Donation { get; set; } = null!;

        [ForeignKey(nameof(Campaign))]
        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; } = null!;


        [ForeignKey(nameof(Donor))]
        public int DonorId { get; set; }
        public virtual Donor Donor { get; set; } = null!;
    }
}
