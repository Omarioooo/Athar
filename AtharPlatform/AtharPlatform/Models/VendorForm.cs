using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace AtharPlatform.Models
{
    public class VendorForm
    {
        [Key]
        public int Id { get; set; }

        // Vendor Information
        [Required, MaxLength(150)]
        public string VendorName { get; set; }

        [Phone]
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        // Offer Details
        [Required, MaxLength(500)]
        public string ItemName { get; set; }

        public int Quantity { get; set; }

        [Required]
        public string Description { get; set; }//To write message for the charity

        [Range(0, double.MaxValue)]
        public decimal PriceBeforDiscount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal PriceBeAfterDiscount { get; set; }

        public DateTime DonationDate { get; set; } 

        public List<CharityVendorOffer> CharityVendorOffer { get; set; }


    }
}
