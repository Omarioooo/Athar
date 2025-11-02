namespace AtharPlatform.DTO
{
    public class DonationDto
    {
        public int DonorId { get; set; }
        public int CharityOrCampaignId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal NetAmountToCharity { get; set; }
    }
}