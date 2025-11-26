namespace AtharPlatform.DTOs
{
    public class ContentPagedResponseDTO
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public List<ContentListDTO> Data { get; set; }
        public string ShareLink { get; set; }
    }
}
