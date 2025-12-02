namespace AtharPlatform.DTOs
{
    public class DonationsHistoryDto
    {
        public string CharityName { get; set; }
        public string Currency { get; set; }
        public DateTime DonationTime { get; set; }

        public double DonationAmount { get; set; }
    }
}
