namespace AtharPlatform.DTOs
{
    public class DonorUpdateDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public IFormFile? Image { get; set; }
    }
}
