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
        [ForeignKey("Charity")]
        public int CharityId { get; set; }

        public virtual Charity Charity { get; set; }

        // Navigation Property
        public virtual List<VendorOffers> VendorOffers { get; set; }
    }
}
