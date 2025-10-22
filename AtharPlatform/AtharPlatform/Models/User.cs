using System.ComponentModel.DataAnnotations;

namespace AtharPlatform.Models
{
    public class Donor : UserAccount
    {
        [Required]
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Country { get; set; }

        public string? City { get; set; }

        public string StripCustomerId { get; set; }

        public List<CampaignDonation> campaignDonations { get; set; }
        public List<UserContetReaction> userContetReaction { get; set; }

        public List<Subscription> subscriptions { get; set; }
    }
}
