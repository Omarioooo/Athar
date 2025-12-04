using System.ComponentModel.DataAnnotations;

namespace AtharPlatform.DTO
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username or email is required")]
        public string UserNameOrEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = string.Empty;

    }
}
