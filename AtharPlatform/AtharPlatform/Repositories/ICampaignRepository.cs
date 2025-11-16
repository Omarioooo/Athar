using AtharPlatform.Models;
using AtharPlatform.Models.Enum;

namespace AtharPlatform.Repositories
{
    public interface ICampaignRepository : IRepository<Campaign>
    {
        Task<List<Campaign>> Getallforusers();
        Task<List<Campaign>> GetByType(CampaignCategoryEnum type);
        Task<List<Campaign>> Search(string keyword);
        Task<List<Campaign>> GetPaginated(int page, int pageSize);
        void Update(Campaign entity);
        Task<Campaign?> GetAsyncTousers(int id);
        Task<List<Campaign>> GetByTypetousers(CampaignCategoryEnum type);
    }
}