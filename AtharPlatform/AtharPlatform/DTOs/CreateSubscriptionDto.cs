namespace AtharPlatform.DTOs
{
    public class CreateSubscriptionDto
    {
        public int DonorId { get; set; }
        public int CharityId { get; set; }
        public decimal Amount { get; set; }
        public string Frequency { get; set; } = "Monthly";
        public string DonorFirstName { get; set; }
        public string DonorLastName { get; set; }
        public string DonorEmail { get; set; }
    }

}
