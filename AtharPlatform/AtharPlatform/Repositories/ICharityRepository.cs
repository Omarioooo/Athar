using AtharPlatform.Models;

namespace AtharPlatform.Repositories
{
    public interface ICharityRepository : IRepository<Charity>
    {
        Task<List<Charity>> GetPageAsync(string? query, int page, int pageSize);
        Task<int> CountAsync(string? query);
        Task<Charity?> GetWithCampaignsAsync(int id);
        Task BulkImportAsync(IEnumerable<Charity> charities);
    }
}