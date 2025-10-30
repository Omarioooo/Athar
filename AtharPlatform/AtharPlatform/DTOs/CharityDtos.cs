using System.ComponentModel.DataAnnotations;
using AtharPlatform.Models;

namespace AtharPlatform.Dtos
{
    // Unified card/detail DTO for charities (used across list/detail/search)
    public class CharityCardDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        // Manual image bytes (for charities created inside the platform)
        public byte[]? Image { get; set; }
        // Scraped image URL (from external sources)
        public string? ImageUrl { get; set; }
        public string? ExternalWebsiteUrl { get; set; }
        public string? MegaKheirUrl { get; set; }
        public int CampaignsCount { get; set; }
        public IEnumerable<MiniCampaignDto> Campaigns { get; set; } = Array.Empty<MiniCampaignDto>();
    }

    // Public, lightweight card view for lists
    public class CharityListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte[]? Image { get; set; }
        public string? ImageUrl { get; set; }
        public int CampaignsCount { get; set; }
    }

    // Detailed view when a user opens a charity page
    public class CharityDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public byte[]? Image { get; set; }
        public string? ImageUrl { get; set; }
        public string? ExternalWebsiteUrl { get; set; }
        public string? MegaKheirUrl { get; set; }
        public IEnumerable<MiniCampaignDto> Campaigns { get; set; } = Array.Empty<MiniCampaignDto>();
    }

    public class MiniCampaignDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public double GoalAmount { get; set; }
        public double RaisedAmount { get; set; }
    }

    // Used for manual creation by Charity Admins or Super Admins
    public class CharityCreateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public byte[]? Image { get; set; }
    }

    // Import contract for bulk-scraped charities
    public class CharityImportItemDto
    {
         
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        // accept base64 image bytes if present (optional); callers can omit
        public byte[]? Image { get; set; }
        // Alternatively, callers can provide a remote image URL
        public string? ImageUrl { get; set; }
        public string? ExternalWebsiteUrl { get; set; }
        public string? MegaKheirUrl { get; set; }
        // Any external id/source can be carried here for dedupe (optional)
        public string? ExternalId { get; set; }
    }

    public class PaginatedResultDto<T>
    {
        public IEnumerable<T> Items { get; set; } = Array.Empty<T>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)Total / Math.Max(1, PageSize));
    }
}
