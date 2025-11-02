using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class Content
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(300)]
        public string Title { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public byte[]? PostImage { get; set; }

        [ForeignKey(nameof(Campaign))]
        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; } = null!;

        public virtual List<Reaction> Reactions { get; set; } = new();
    }

}
