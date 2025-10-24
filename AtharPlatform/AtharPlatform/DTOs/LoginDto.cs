using System.ComponentModel.DataAnnotations;

namespace AtharPlatform.DTO
{
    public class LoginDto
    {
        [Required]
        public string? UserNameOrEmail { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
