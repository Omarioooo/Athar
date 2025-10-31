using AtharPlatform.Models;
using AtharPlatform.Models.Enum;

namespace AtharPlatform.Repositories
{
    public interface ICampaignRepository : IRepository<Campaign>
    {
        Task<List<Campaign>> GetPageAsync(string? query, int page, int pageSize);
        Task<int> CountAsync(string? query);
        Task<List<Campaign>> GetByDateAsync(bool latestFirst);

        Task<List<Campaign>> GetByType(CampaignCategoryEnum type);

    }
}
