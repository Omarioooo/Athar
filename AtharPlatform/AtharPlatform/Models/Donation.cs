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
        public decimal NetAmountToCharity { get; set; }// Amount charity actually receives


        public string PaymentReference { get; set; }//strip_PaymentId


        [Required]
        public string DonationStatus { get; set; }// Pending, Completed, Failed

        public DateTime CreatedAt { get; set; }

        public List<CharityDonation> CharityDonations { get; set; }
        public List<CampaignDonation> CampaignDonations { get; set; }

    }
}
