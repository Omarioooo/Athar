using System.ComponentModel.DataAnnotations;

namespace AtharPlatform.DTO
{
    public class DonorRegisterDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } = "Donor";

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        public IFormFile? ProfileImage { get; set; }
    }
}
