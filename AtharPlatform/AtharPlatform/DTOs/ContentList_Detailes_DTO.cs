namespace AtharPlatform.DTOs
{
    public class ContentList_Detailes_DTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // الحملة والجمعية
        public int CampaignId { get; set; }
        public string CampaignTitle { get; set; }
        public int CharityId { get; set; }
        public string CharityName { get; set; }
    }
}
