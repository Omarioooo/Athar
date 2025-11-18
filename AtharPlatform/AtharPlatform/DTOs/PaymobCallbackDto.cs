namespace AtharPlatform.DTOs
{
    public class PaymobCallbackDto
    {
        public string? obj { get; set; }
        public bool success { get; set; }
        public string? transactionId { get; set; }
        public string? paymentId { get; set; }
        public decimal amount { get; set; }
    }
}
