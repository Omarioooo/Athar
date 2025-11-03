namespace AtharPlatform.DTOs
{
    public class PaginatedResultDto<T>
    {
        public IEnumerable<T> Items { get; set; } = Array.Empty<T>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)Total / Math.Max(1, PageSize));
    }
}
