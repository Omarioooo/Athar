using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class Donation
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        public decimal PlatformFee { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        public decimal NetAmountToCharity { get; set; }

        public string PaymentReference { get; set; }

        [Required]
        public string DonationStatus { get; set; }

        public virtual DateTime CreatedAt { get; set; } = new();

        public virtual List<CharityDonation> CharityDonations { get; set; } = new();
        public virtual List<CampaignDonation> CampaignDonations { get; set; } = new();

    }
}
