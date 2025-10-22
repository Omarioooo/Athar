using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class CampaignContent
    {
        [ForeignKey(nameof(Campaign))]
        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; } = new();



        [Key, ForeignKey(nameof(Content))]
        public int ContentId { get; set; }
        public virtual Content Content { get; set; } = new();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
