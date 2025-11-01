using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AtharPlatform.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace AtharPlatform.Models
{
    public class Charity : UserAccount
    {
        [Key]
        [ForeignKey(nameof(Account))]
        public int Id { get; set; }

        [Required]
        public String Name { get; set; }

        [Required]
        public String Description { get; set; }


        [Column(TypeName = "decimal(18, 4)")]
        public decimal? Balance { get; set; } = 0.0m;

        [Required]
        public byte[]? VerificationDocument { get; set; }
        public CharityStatusEnum Status { get; set; } = CharityStatusEnum.Pending;

        public virtual UserAccount Account { get; set; } = new();
        public virtual List<CharityVolunteer> charityVolunteers { get; set; } = new();
        public virtual List<CharityVendorOffer> charityVendorOffers { get; set; } = new();
        public virtual List<CharityMaterialDonation> charityMaterialDonations { get; set; } = new();
        public virtual List<Follow> Follows { get; set; } = new();
        public virtual List<Subscription> subscriptions { get; set; } = new();
        public virtual List<Campaign> campaigns { get; set; } = new();
        public virtual List<Donation> donations { get; set; } = new();
    }
}
