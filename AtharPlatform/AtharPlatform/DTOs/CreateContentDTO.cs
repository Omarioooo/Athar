using System.ComponentModel.DataAnnotations;

namespace AtharPlatform.DTOs
{
    public class CreateContentDTO
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
        [Required(ErrorMessage = "CampaignId is required")]
        public int CampaignId { get; set; }
        [Required]
        public IFormFile? PostImage { get; set; }  
    }
}
