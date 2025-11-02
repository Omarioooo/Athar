using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Donor))]
        public int DonorId { get; set; }
        public virtual Donor Donor { get; set; } = null!;

        [ForeignKey(nameof(Charity))]
        public int CharityId { get; set; }
        public virtual Charity Charity { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public string Frequency { get; set; } = "Monthly";

        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastPaymentDate { get; set; }
        public DateTime? NextPaymentDate { get; set; }
    }
}