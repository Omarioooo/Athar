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

        public bool IsDeleted { get; set; }

        public string Description { get; set; }

        public byte[] MediaUrl { get; set; }// image for the post


        public DateTime CreatedAt { get; set; }



        public List<CampaignContent> CampaignContent { get; set; }
        public List<UserContetReaction> UserContetReaction { get; set; }
       
    }
}
