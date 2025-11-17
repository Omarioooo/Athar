using System.ComponentModel.DataAnnotations;

namespace AtharPlatform.DTO
{
    public class CreatePaymentDto
    {
        public int DonorId { get; set; }

        [Required]
        public string DonorFirstName { get; set; }

        [Required]
        public string DonorLastName { get; set; }

        [Required]
        public string DonorEmail { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public string Currency { get; set; } = "EGP";
        public string? MerchantOrderId { get; set; }
        public int CampaignId { get; set; }
        public int CharityId { get; set; }
    }
}
