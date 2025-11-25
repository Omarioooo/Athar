using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class CharityVendorOffer
    {
        [Key]
        public int CharityVendorOfferId { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [Required]
        [ForeignKey(nameof(Charity))]
        public int CharityId { get; set; }

        public bool IsOpen { get; set; } = true;

        public virtual Charity Charity { get; set; } = null!;

        // Navigation Property
        public virtual List<VendorOffers> VendorOffers { get; set; } = null!;
    }
}
