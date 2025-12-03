using AtharPlatform.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class Donor
    {
        [Key]
        [ForeignKey(nameof(Account))]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; }

        public RolesEnum Role { get; set; } = RolesEnum.Donor;

        public virtual UserAccount Account { get; set; } = null!;
        public virtual List<CampaignDonation> Donations { get; set; } = null!;
        public virtual List<Reaction> Reactions { get; set; } = null!;
        public virtual List<Subscription> Subscriptions { get; set; } = null!;
        public virtual List<Follow> Follows { get; set; } = null!;
    }
}
