using System.ComponentModel.DataAnnotations;

namespace AtharPlatform.DTOs
{
    public class UpdateCharityDto
    {
        public string CharityName { get; set; }

       public string Description { get; set; }

       public string Country { get; set; }

        public string City { get; set; }

        public IFormFile? ProfileImage { get; set; }
    }
}
