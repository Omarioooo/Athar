namespace AtharPlatform.DTOs
{
    public class CharityViewDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public int NumOfFollowers { get; set; }
        public int NumOfCampaigns { get; set; }
        public List<CharityViewCampaignDto> Campaigns { get; set; }
        public List<CharityViewContentDto> Contents { get; set; }
    }

    public class CharityViewCampaignDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImgUrl { get; set; }
        public string Description { get; set; }
        public double RaisedAmount { get; set; }
        public double GoaldAmount { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

    }


    public class CharityViewContentDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }

        public int TotalReactions { get; set; }
    }

}
