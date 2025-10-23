using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    // Temporary entity to hold uploaded beneficiary rows for validation and de-duplication
    public class BeneficiaryUpload
    {
        [Key]
        public int Id { get; set; }

        // Optional association to a campaign and charity for context
        public int? CampaignId { get; set; }
        public virtual Campaign? Campaign { get; set; }

        public string? CharityId { get; set; } // Charity inherits UserAccount (string key)
        public virtual Charity? Charity { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        // Used for cross-charity de-duplication checks
        [Required]
        [MaxLength(50)]
        public string NationalId { get; set; } = string.Empty;

        [Phone]
        [MaxLength(30)]
        public string? Phone { get; set; }

        // Flags to assist validation workflow
        public bool IsPotentialDuplicate { get; set; } = false;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }
}
