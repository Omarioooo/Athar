using AtharPlatform.Models.Enum;
using System.Linq.Expressions;
namespace AtharPlatform.Repositories
{
    public class CampaignRepository : Repository<Campaign>, ICampaignRepository
    {
        public CampaignRepository(Context context) : base(context)
        {
        }


        public override async Task<Campaign> GetAsync(int id)
        {
            var result = await base.GetAsync(id);
            if (result == null)
                throw new InvalidOperationException("Campaign not found.");
            return result;
        }

        public override async Task<Campaign?> GetAsync(Expression<Func<Campaign, bool>> expression)
        {
            var result = await base.GetAsync(expression);
            if (result == null)
                throw new InvalidOperationException("Campaign not found.");
            return result;
        }
        public override async Task<List<Campaign>> GetAllAsync()
        {
              return await _dbSet
                .Include(c => c.Charity) 
                .ToListAsync();
        }

        public async Task<List<Campaign>> GetByType(CampaignCategoryEnum type)
        {
            var result = await _dbSet
               .Where(c => c.Category == type)
               .ToListAsync();
            if (result == null)
                throw new InvalidOperationException("Campaign not found.");
            return result;

        }
    }
}
