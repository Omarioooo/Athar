using AtharPlatform.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.DTOs
{
    public class CharityJoinDto
    {
        public int Id { get; set; }
        public String Name { get; set; } = string.Empty;
        public String email { get; set; } = string.Empty;

        public String Description { get; set; } = string.Empty;

        public string? VerificationDocument { get; set; }

        public string? ImageUrl { get; set; }

        public string? Country { get; set; }

        public string? City { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
