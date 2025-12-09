namespace AtharPlatform.DTOs
{
    public class CharityStatusDto
    {
        public int FollowsCount { get; set; }
        public List<StatsFollowDto>? Follows { get; set; }

        public int ReactionsCount { get; set; }
        public List<ReactionDto> Reactions { get; set; }

        public int ContentCount { get; set; }

        public int DonationsCount { get; set; }
        public decimal TotalIncome { get; set; }
        public List<DonationInfoDto> Donations { get; set; }

        public int CampaignsCount { get; set; }
        public List<CampaignCategoryCountDto> CampaignsByCategory { get; set; }
    }
    
}
