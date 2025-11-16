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
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Column(TypeName = "decimal(18, 4)")]
        public decimal? Balance { get; set; } = 0.0m;

        // Verification document uploaded during registration
        public byte[]? VerificationDocument { get; set; }

        public CharityStatusEnum Status { get; set; } = CharityStatusEnum.Pending;

        // Optional presentation image/logo to show on cards
        public byte[]? Image { get; set; }

        // Scraped external presentation fields moved to a 1:1 entity
        public virtual CharityExternalInfo? ScrapedInfo { get; set; }

        // Distinguish whether data was imported from scraping or created manually
        public bool IsScraped { get; set; } = false;
        public string? ExternalId { get; set; }
        public DateTime? ImportedAt { get; set; }

        // Soft delete / deactivate flag
        public bool IsActive { get; set; } = true;
        public DateTime? DeactivatedAt { get; set; }

        public virtual UserAccount Account { get; set; } = null!;
        public virtual List<CharityVolunteer> CharityVolunteers { get; set; } = null!;
        public virtual List<CharityVendorOffer> CharityVendorOffers { get; set; } = null!;
        public virtual List<CharityMaterialDonation> CharityMaterialDonations { get; set; } = null!;
        public virtual List<Follow> Follows { get; set; } = null!;
        public virtual List<Subscription> Subscriptions { get; set; } = null!;
        public virtual List<Campaign> Campaigns { get; set; } = null!;
        public virtual List<Donation> Donations { get; set; } = null!;
    }
}
