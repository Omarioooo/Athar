using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AtharPlatform.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace AtharPlatform.Models
{
    public class Charity
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

        public virtual UserAccount Account { get; set; }
        public virtual List<CharityVolunteer> CharityVolunteers { get; set; } = null!;
        public virtual List<CharityVendorOffer> CharityVendorOffers { get; set; } = null!;
        public virtual List<CharityMaterialDonation> CharityMaterialDonations { get; set; } = null!;
        public virtual List<Follow> Follows { get; set; } = null!;
        public virtual List<Subscription> Subscriptions { get; set; } = null!;
        public virtual List<Campaign> Campaigns { get; set; } = null!;
        public virtual List<Donation> Donations { get; set; } = null!;
    }
}
