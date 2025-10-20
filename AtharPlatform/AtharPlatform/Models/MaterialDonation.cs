using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
  public enum MaterialStatus 
    {
        New,
        Used
    }
    public class MaterialDonation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DonorFirstName { get; set; }
        [Required]
        public string DonorLastName { get; set; }

        [Required]
        public string ItemName { get; set; }

        public DateTime DonationDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required, MaxLength(500)]
        public string Description { get; set; }

        public MaterialStatus Condition { get; set; } // e.g., New, Used, Good Condition

        [Required]
        public string PickupAddress { get; set; }// (Address)

        public string MeasurementUnit { get; set; }


        public List<DonationImage> Images { get; set; } = new List<DonationImage>();//Photos for the matrials

        public List<CharityMaterialDonation> charityMaterialDonations { get; set; }
        //Uplod images for the material
    }
    public class DonationImage
    {
        [Key]
        public int Id { get; set; }
        public byte[] ImageData { get; set; }

        [Required]
        public int MaterialDonationId { get; set; }

        [ForeignKey("MaterialDonationId")]
        public MaterialDonation MaterialDonation { get; set; }
    }

}
