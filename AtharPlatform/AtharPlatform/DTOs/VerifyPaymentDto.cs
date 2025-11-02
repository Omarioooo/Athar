using System.ComponentModel.DataAnnotations;

namespace AtharPlatform.DTO
{
    public class VerifyPaymentDto
    {
        [Required]
        public string TransactionId { get; set; }

        [Required]
        public string DonationMerchantOrderId { get; set; }
        public string? Hmac { get; set; }
    }
}
