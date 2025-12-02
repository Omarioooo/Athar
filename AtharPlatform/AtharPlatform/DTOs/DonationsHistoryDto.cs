namespace AtharPlatform.DTOs
{
    public class DonationsHistoryDto
    {
        public int DonationId { get; set; }         
        public double Amount { get; set; }            
        public DateTime? DonationDate { get; set; }  
        public string? Currency { get; set; }       
        public string? Status { get; set; }          
        public int CampaignId { get; set; }         
        public int CharityId { get; set; }          
    }
}
