using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class CampaignDonation
    {

        [Key, ForeignKey(nameof(Donation))]
        public int DonationId { get; set; }
        public virtual Donation Donation { get; set; } = new();

        public DateTime Date { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(Campaign))]
        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; } = new();


    [ForeignKey(nameof(Donor))]
        public String DonorId { get; set; }
        public virtual Donor Donor { get; set; } = new();
    }
}
