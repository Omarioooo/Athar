using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class CampaignContent
    {
        [ForeignKey("Campaign")]
        public int ContentId { get; set; }
        public Campaign Campaign { get; set; }



        [ForeignKey("Content")]
        public int Id { get; set; }
        public Content Content { get; set; }



        public DateTime CreatedAt { get; set; }
    }
}
