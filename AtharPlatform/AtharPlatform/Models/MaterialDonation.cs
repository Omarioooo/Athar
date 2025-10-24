using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class MaterialDonation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DonorFirstName { get; set; }
        [Required]
        public string DonorLastName { get; set; }

        [Phone]
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }

        [Required]
        public string MeasurementUnit { get; set; }


        [ForeignKey(nameof(CharityMaterialDonation))]
        public int MaterialDonationId { get; set; }

        public virtual CharityMaterialDonation CharityMaterialDonation { get; set; } = new();
    }
}
