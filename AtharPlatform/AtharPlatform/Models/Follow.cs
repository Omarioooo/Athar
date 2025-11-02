using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class Follow
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Donor))]
        public int DonorId { get; set; }
        public virtual Donor Donor { get; set; } = null!;

        [ForeignKey(nameof(Charity))]
        public int CharityId { get; set; }
        public virtual Charity Charity { get; set; } = null!;

        public DateTime StartDate { get; set; } = DateTime.UtcNow;
    }
}
