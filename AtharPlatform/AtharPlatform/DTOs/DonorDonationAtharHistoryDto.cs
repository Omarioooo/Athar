namespace AtharPlatform.DTOs
{
    public class DonorDonationAtharHistoryDto
    {
        public int DonationId { get; set; }
        public string CharityName { get; set; }
        public string CampaignName { get; set; }
        public decimal Amount { get; set; }
        public DateTime? Date { get; set; }
    }
}
