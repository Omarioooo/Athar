using System.ComponentModel.DataAnnotations;

namespace AtharPlatform.DTO
{
    public class PersonRegisterDto
    {
        [Required]
        public string FirstName { get; set; }

        public string? LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string? Country { get; set; }

        public string? City { get; set; }

        public IFormFile? ProfileImage { get; set; }
    }
}
