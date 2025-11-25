namespace AtharPlatform.DTO
{
    public class PaymentOutPutDto
    {
        public int DonationId { get; set; }
        public string? PaymentUrl { get; set; }
        public string? PaymentId { get; set; }
        public string MerchantOrderId { get; set; }
    }
}
