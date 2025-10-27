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

        public string? PaymobWalletId { get; set; }

        public decimal AvailableBalance { get; set; } = 0;

        [Required]
        public byte[] VerificationDocument { get; set; }
        public CharityStatusEnum Status { get; set; } = CharityStatusEnum.Pending;

        public virtual UserAccount Account { get; set; }
        public virtual List<CharityVolunteer> charityVolunteers { get; set; } = new();
        public virtual List<CharityVendorOffer> charityVendorOffers { get; set; } = new();
        public virtual List<CharityMaterialDonation> charityMaterialDonations { get; set; } = new();
        public virtual List<Subscription> subscriptions { get; set; } = new();
        public virtual List<Campaign> campaigns { get; set; } = new();

    }
}
