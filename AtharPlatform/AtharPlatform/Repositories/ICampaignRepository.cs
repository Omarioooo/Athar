using AtharPlatform.Models;
using AtharPlatform.Models.Enum;

namespace AtharPlatform.Repositories
{
    public interface ICampaignRepository : IRepository<Campaign>
    {

        Task<List<Campaign>>GetByType(CampaignCategoryEnum type);
        Task<List<Campaign>> GetByTypetousers(CampaignCategoryEnum type);
        Task<IEnumerable<Campaign>> Search(string keyword);
        Task<IEnumerable<Campaign>> GetPaginated(int page, int pageSize);
        Task<List<Campaign>> Getallforusers();
        Task<Campaign> GetAsyncTousers(int id);

    }
}
