namespace AtharPlatform.DTOs
{
    public class PaginatedResultDto<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)Total / PageSize);
    }
}
