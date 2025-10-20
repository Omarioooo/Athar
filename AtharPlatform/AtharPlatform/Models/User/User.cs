namespace AtharPlatform.Models
{
    public class User : UserAccount
    {
        public string Address { get; set; }
        public string StripCustomerId { get; set; }

         public List<CampaignDonation> campaignDonations { get; set; }
    }
}
