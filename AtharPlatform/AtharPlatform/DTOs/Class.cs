// Obsolete duplicate. Use AtharPlatform.Dtos.PaginatedResultDto<T> defined in DTOs/CharityDtos.cs
// Keeping this file as a no-op placeholder to avoid breaking history; it should be removed in a future cleanup.
namespace AtharPlatform.DTOs
{
    [Obsolete("Use AtharPlatform.Dtos.PaginatedResultDto<T> instead.")]
    internal class PaginatedResultDtoObsolete<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)Total / Math.Max(1, PageSize));
    }
}
