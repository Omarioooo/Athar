using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class Reaction

    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime ReactionDate { get; set; }

        [ForeignKey(nameof(Donor))]
        public int DonorID { get; set; }
        public virtual Donor Donor { get; set; }

        [ForeignKey(nameof(Content))]
        public int ContentID { get; set; }
        public virtual Content Content { get; set; } = new();
    }
}
