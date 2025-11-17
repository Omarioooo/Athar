namespace AtharPlatform.Repositories
{
    public class CampaignDonationRepository : Repository<CampaignDonation>, ICampaignDonation
    {
        public CampaignDonationRepository(Context context) : base(context) { }
    }
}
