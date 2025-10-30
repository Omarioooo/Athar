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

    [Required]
    public byte[] VerificationDocuments { get; set; }

    // Optional presentation image/logo to show on cards
    public byte[]? Image { get; set; }

    // Scraped external presentation fields moved to a 1:1 entity
    public virtual CharityExternalInfo? ScrapedInfo { get; set; }

    // Distinguish whether data was imported from scraping or created manually
    public bool IsScraped { get; set; } = false;
    public string? ExternalId { get; set; }
    public DateTime? ImportedAt { get; set; }

        public virtual UserAccount Account { get; set; }
        public CharityStatusEnum Status { get; set; } = CharityStatusEnum.Pending;
        public virtual List<CharityVolunteer> charityVolunteers { get; set; } = new();
        public virtual List<CharityVendorOffer> charityVendorOffers { get; set; } = new();
        public virtual List<CharityMaterialDonation> charityMaterialDonations { get; set; } = new();
        public virtual List<Subscription> subscriptions { get; set; } = new();
        public virtual List<Campaign> campaigns { get; set; } = new();

    }
}
