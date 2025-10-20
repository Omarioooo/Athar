using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.Models
{
    public class CampaignContent
    {
        [ForeignKey("Campaign")]
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }



        [Key,ForeignKey("Content")]
        public int ContentId { get; set; }
        public Content Content { get; set; }



        public DateTime CreatedAt { get; set; }
    }
}
