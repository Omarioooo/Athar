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
                    (c.Charity != null && c.Charity.Name.Contains(term)));
            }

            return await q
                .OrderBy(c => c.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Campaign>> Search(string keyword)
        {
            var q = _dbSet
                .Include(c => c.Charity)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var term = keyword.Trim();
                q = q.Where(c =>
                    c.Title.Contains(term) ||
                    c.Description.Contains(term) ||
                    (c.Charity != null && c.Charity.Name.Contains(term)));
            }

            q = q.Where(c => c.Status == CampainStatusEnum.inProgress);

            return await q.ToListAsync();
        }

        public async Task<IEnumerable<Campaign>> GetPaginated(int page, int pageSize)
        {
            return await _context.Campaigns
                .Where(c => c.Status == CampainStatusEnum.inProgress)
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Campaign>> Getallforusers()
        {
            return await _dbSet
                .Where(c => c.Status == CampainStatusEnum.inProgress)
                .Include(c => c.Charity)
                .ToListAsync();
        }

        public async Task<Campaign> GetAsyncTousers(int id)
        {
            var result = await _dbSet
                .Where(c => (c.Id == id && c.Status == CampainStatusEnum.inProgress))
                .Include(c => c.Charity)
                .FirstOrDefaultAsync();

            if (result == null)
                throw new InvalidOperationException("Campaign not found.");

            return result;
        }

        public async Task<List<Campaign>> GetByTypetousers(CampaignCategoryEnum type)
        {
            var result = await _dbSet
                .Where(c => (c.Category == type && c.Status == CampainStatusEnum.inProgress))
                .ToListAsync();

            if (result == null)
                throw new InvalidOperationException("Campaign not found.");

            return result;
        }
    }
}
