using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    // Holds scraped-only metadata for a charity in a 1:1 relation
    public class CharityExternalInfo
    {
        [Key]
        [ForeignKey(nameof(Charity))]
        public int CharityId { get; set; }

        public string? ImageUrl { get; set; }
        public string? ExternalWebsiteUrl { get; set; }
        public string? MegaKheirUrl { get; set; }

        public virtual Charity Charity { get; set; } = null!;
    }
}
