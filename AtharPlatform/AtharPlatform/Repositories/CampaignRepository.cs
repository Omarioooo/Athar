using System.Linq.Expressions;
using AtharPlatform.Models.Enum;
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
            return await _dbSet
            .Where(c => c.Category == type)
            .Include(c => c.Charity)
            .ToListAsync();
        }

        public async Task<List<Campaign>> GetPageAsync(string? query, int page, int pageSize)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 12;

            var q = _dbSet
                .Include(c => c.Charity)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                var term = query.Trim();
                q = q.Where(c =>
                    c.Title.Contains(term) ||
                    c.Description.Contains(term) ||
                    c.Charity.Name.Contains(term));
            }

            return await q
                .OrderBy(c => c.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountAsync(string? query)
        {
            var q = _dbSet.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                var term = query.Trim();
                q = q.Where(c =>
                    c.Title.Contains(term) ||
                    c.Description.Contains(term) ||
                    c.Charity.Name.Contains(term));
            }

            return await q.CountAsync();
        }

        public  async Task<List<Campaign>> GetByDateAsync(bool latestFirst)
        {
            var query = _context.Campaigns
                    .Include(c => c.Charity)
                    .AsQueryable();

            if (latestFirst)
                query = query.OrderByDescending(c => c.StartDate);
            else
                query = query.OrderBy(c => c.StartDate);

            return await query.ToListAsync();
        }


    }
}
