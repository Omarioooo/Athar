using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class Donor : UserAccount
    {
        [Key]
        [ForeignKey(nameof(Account))]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string? LastName { get; set; }

        public virtual UserAccount Account { get; set; }
        public virtual List<CampaignDonation> Donations { get; set; } = new();
        public virtual List<Reaction> Reactions { get; set; } = new();
        public virtual List<Subscription> Subscriptions { get; set; } = new();
        public virtual List<Follow> Follows { get; set; } = new();
    }
}
