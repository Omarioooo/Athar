using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class CampaignDonation
    {

        [Key, ForeignKey(nameof(Donation))]
        public int DonationId { get; set; }
        public Donation Donation { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(Campaign))]
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }


        [ForeignKey(nameof(Donor)]
        public String DonorId { get; set; }
        public Donor Donor { get; set; }


    }
}
