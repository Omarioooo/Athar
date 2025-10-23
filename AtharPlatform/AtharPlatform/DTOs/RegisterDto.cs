using System.ComponentModel.DataAnnotations;

namespace AtharPlatform.DTO
{
    public class RegisterDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        // Client
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        // Market
        public string? MarketName { get; set; }
        public string? Description { get; set; }
    }
}
