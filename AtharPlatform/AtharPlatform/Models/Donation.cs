using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AtharPlatform.Models
{
    [Index(nameof(StripePaymentId), IsUnique = true)]
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

        // Payment provider details (Stripe, etc.)
        public string? Provider { get; set; }
        // Stripe idempotency/payment identifiers to avoid duplicate processing
        public string? StripePaymentId { get; set; }
        // Webhook processing guard
        public bool IsWebhookProcessed { get; set; } = false;
        public virtual DateTime CreatedAt { get; set; } = new();
        public virtual List<CharityDonation> CharityDonations { get; set; } = new();
        public virtual List<CampaignDonation> CampaignDonations { get; set; } = new();

    }
}
