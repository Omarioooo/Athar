using AtharPlatform.Models;
using AtharPlatform.Models.Enum;
namespace AtharPlatform.Repositories
{
    public interface ICampaignRepository : IRepository<Campaign>
    {
        Task<List<Campaign>>GetByType(CampaignCategoryEnum type);
    }
}
