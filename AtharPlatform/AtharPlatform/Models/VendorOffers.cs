using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AtharPlatform.Models.Enums;

namespace AtharPlatform.Models
{
    public class VendorOffers
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string VendorName { get; set; }

        [Phone]
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceBeforDiscount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PriceAfterDiscount { get; set; }

        [Required]
        public OfferStatus Status { get; set; }= OfferStatus.Pending;

        [Required]
        [ForeignKey(nameof(CharityVendorOffer))]
        public int CharityVendorOfferId { get; set; }
        public virtual CharityVendorOffer CharityVendorOffer { get; set; } = null!;
    }
}
