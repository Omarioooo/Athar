using AtharPlatform.Models;

namespace AtharPlatform.Repositories
{
    public class CampaignRepository : Repository<Campaign>, ICampaignRepository
    {
        public CampaignRepository(Context context) : base(context)
        {
        }
    }
}
