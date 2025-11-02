using AtharPlatform.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;


namespace AtharPlatform.Models
{
    public class Donation
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Donor))]
        public int DonorId { get; set; }
        public virtual Donor Donor { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        // Platform fee is 2%
        [NotMapped]
        public decimal PlatformFee { get; set; } = 0.02m;

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetAmountToCharity { get; set; }

        public string Currency { get; set; } = "EGP";

        public TransactionStatusEnum DonationStatus { get; set; } = TransactionStatusEnum.PENDING;

        // payment IDs from Paymob...
        public string? PaymentID { get; set; }
        public string? MerchantOrderId { get; set; }
        public string? TransactionId { get; set; }

        public virtual DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual List<CharityDonation> CharityDonations { get; set; } = null!;
        public virtual List<CampaignDonation> CampaignDonations { get; set; } = null!;
    }
}
