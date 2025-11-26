namespace AtharPlatform.DTOs
{
    public class ContentListDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string ShareLink { get; set; }
    }
}
