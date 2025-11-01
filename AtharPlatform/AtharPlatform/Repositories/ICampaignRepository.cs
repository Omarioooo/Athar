using AtharPlatform.Models;
using AtharPlatform.Models.Enum;
<<<<<<< HEAD
=======

>>>>>>> master
namespace AtharPlatform.Repositories
{
    public interface ICampaignRepository : IRepository<Campaign>
    {
<<<<<<< HEAD
        Task<List<Campaign>>GetByType(CampaignCategoryEnum type);
        Task<List<Campaign>> GetByTypetousers(CampaignCategoryEnum type);
        Task<IEnumerable<Campaign>> Search(string keyword);
        Task<IEnumerable<Campaign>> GetPaginated(int page, int pageSize);
        Task<List<Campaign>> Getallforusers();
        Task<Campaign> GetAsyncTousers(int id);
=======
        Task<List<Campaign>> GetPageAsync(string? query, int page, int pageSize);
        Task<int> CountAsync(string? query);
        Task<List<Campaign>> GetByDateAsync(bool latestFirst);

        Task<List<Campaign>> GetByType(CampaignCategoryEnum type);

>>>>>>> master
    }
}
