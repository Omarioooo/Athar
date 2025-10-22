using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{

    //Weak Entity
    public class CharityMaterialDonation
    {
        [Key]
        public int MaterialDonationId { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [Required]
        [ForeignKey(nameof(Charity))]
        public int CharityId { get; set; }

        public virtual Charity Charity { get; set; } = new();

        // Navigation Property
        public virtual List<MaterialDonation> MaterialDonations { get; set; } = new();
    }
}
