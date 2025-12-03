namespace AtharPlatform.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = "Donor";
    }

}
