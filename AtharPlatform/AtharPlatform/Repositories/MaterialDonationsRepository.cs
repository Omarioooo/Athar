
namespace AtharPlatform.Repositories
{
    public class MaterialDonationsRepository : Repository<MaterialDonation>, IMaterialDonationsRepository
    {
        public MaterialDonationsRepository(Context context) : base(context)
        {
        }

        public override async Task<List<MaterialDonation>> GetAllAsync()
        {
            return await _dbSet
                .Include(m => m.CharityMaterialDonation)
                .ToListAsync();
        }

        // Get all applications for a specific volunteer opportunity
        public async Task<List<MaterialDonation>> GetByCampaignAsync(int charityDonationId)
        {
            return await _dbSet
                .Where(m => m.CharityMaterialDonation.MaterialDonationId == charityDonationId)
                .Include(m => m.CharityMaterialDonation)
                .ToListAsync();
        }
    }
}
