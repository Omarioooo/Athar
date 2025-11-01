using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }


        [ForeignKey(nameof(Donor))]
        public int DonorID { get; set; }
        public Donor Donor { get; set; }


        [ForeignKey(nameof(Charity))]
        public int CharityID { get; set; }
        public Charity Charity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public string Frequency { get; set; } = "Monthly";

        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastPaymentDate { get; set; }
        public DateTime? NextPaymentDate { get; set; }
    }
}
