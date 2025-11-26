namespace AtharPlatform.DTOs
{
    public class UpdateContentDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public IFormFile? PostImage { get; set; } // اختيارية
    }
}
