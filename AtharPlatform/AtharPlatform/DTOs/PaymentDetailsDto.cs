using System.ComponentModel.DataAnnotations;

namespace AtharPlatform.DTO
{
    public class PaymentDetailsDto
    {
        public string? PaymobTransactionId { get; set; }
        public string? DonationMerchantOrderId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EGP";
        public DateTime? PaidAt { get; set; }
        public string DonorName { get; set; }
        public string DonorEmail { get; set; }
        public string DonorPhone { get; set; }
    }
}
