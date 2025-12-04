using AtharPlatform.Models;

namespace AtharPlatform.Repositories
{
    public interface IMaterialDonationsRepository : IRepository<MaterialDonation>
    {
        Task<List<MaterialDonation>> GetByCampaignAsync(int id);
    }
}