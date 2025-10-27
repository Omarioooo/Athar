using System.ComponentModel.DataAnnotations;

namespace AtharPlatform.DTOs
{
    public class CharityRegisterDto
    {
        [Required]
        public string CharityName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } = "Charity";

        [Required]
        public string Description { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        public IFormFile? VerificationDocument { get; set; }
        public IFormFile? ProfileImage { get; set; }
    }
}
