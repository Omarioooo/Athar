using System.ComponentModel.DataAnnotations;
using AtharPlatform.Models.Enums;

namespace AtharPlatform.DTOs
{
    public class VendorOfferDTO
    {
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
        public decimal PriceBeforDiscount { get; set; }

        [Required]
        public decimal PriceAfterDiscount { get; set; }

        public OfferStatus Status { get; set; } = OfferStatus.Pending;

        [Required]
        public int CharityVendorOfferId { get; set; }
    }
}
