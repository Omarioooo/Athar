using AtharPlatform.Models.Enums;

namespace AtharPlatform.DTOs
{
    public class DonationInfoDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime? Date { get; set; }
        public TransactionStatusEnum Status { get; set; }
    }
}
