using AtharPlatform.Dtos;
using AtharPlatform.Models.Enums;

namespace AtharPlatform.DTOs
{
    public class CharityProfileDto
    {
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? Address { get; set; }
        public int CampaignsCount { get; set; }
        public double TotalRaised { get; set; }
        public int FollowersCount { get; set; }
        public CharityStatusEnum status { get; set; } = CharityStatusEnum.Pending;

        public IEnumerable<MiniCampaignDto> Campaigns { get; set; } = new List<MiniCampaignDto>();
    }
}
