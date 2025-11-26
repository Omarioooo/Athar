using System.ComponentModel.DataAnnotations;

namespace AtharPlatform.DTOs
{
    public class CreateContentDTO
    {
        public string Title { get; set; }

        public string Description { get; set; }
        [Required(ErrorMessage = "CampaignId is required")]
        public int CampaignId { get; set; }
        public string ShareLink { get; set; }
        public IFormFile? PostImage { get; set; }  
    }
}
