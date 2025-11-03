namespace AtharPlatform.DTOs
{
    public class PaginatedResultDto<T>
    {
        public List<T> Items { get; set; } = null!;
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)Total / Math.Max(1, PageSize));
    }
}
