using System.ComponentModel.DataAnnotations;

namespace AtharPlatform.Models
{
    public class Content
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Title { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? Description { get; set; }
        public byte[]? PostImage { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual List<Reaction> Reactions { get; set; } = new();
    }
}
