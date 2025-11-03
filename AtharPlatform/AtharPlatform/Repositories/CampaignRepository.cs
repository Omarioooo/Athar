using AtharPlatform.Models.Enum;

namespace AtharPlatform.Repositories
{
    public class CampaignRepository : Repository<Campaign>, ICampaignRepository
    {
        public CampaignRepository(Context context) : base(context)
        {
        }

        #region Query Helpers

        private IQueryable<Campaign> BaseQuery(bool includeCharity = false)
        {
            var query = _dbSet.AsQueryable();

            if (includeCharity)
                query = query.Include(c => c.Charity);

            return query;
        }

        private IQueryable<Campaign> InProgressQuery(bool includeCharity = false)
        {
            return BaseQuery(includeCharity)
                .Where(c => c.Status == CampainStatusEnum.inProgress);
        }

        private IQueryable<Campaign> ApplySearchFilter(IQueryable<Campaign> query, string? keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return query;

            var term = keyword.Trim();
            return query.Where(c =>
                c.Title.Contains(term) ||
                c.Description.Contains(term) ||
                (c.Charity != null && c.Charity.Name.Contains(term)));
        }

        #endregion

        public async Task<List<Campaign>> GetAllWithCharitiesAsync()
        {
            return await BaseQuery(includeCharity: true).ToListAsync();
        }

        public async Task<List<Campaign>> GetAllInProgressWithCharitiesAsync()
        {
            return await InProgressQuery(includeCharity: true).ToListAsync();
        }

        public async Task<List<Campaign>> GetAllByTypeAsync(CampaignCategoryEnum type)
        {
            return await BaseQuery(includeCharity: true)
                .Where(c => c.Category == type)
                .ToListAsync();
        }

        public async Task<List<Campaign>> GetAllInProgressByTypeAsync(CampaignCategoryEnum type)
        {
            return await InProgressQuery()
                .Where(c => c.Category == type)
                .ToListAsync();
        }

        public async Task<List<Campaign>> Search(string keyword)
        {
            return await ApplySearchFilter(InProgressQuery(includeCharity: true), keyword)
                .ToListAsync();
        }

        public async Task<List<Campaign>> GetPageAsync(string? query, int page, int pageSize)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Max(pageSize, 12);

            var q = ApplySearchFilter(BaseQuery(includeCharity: true), query);

            return await q
                .OrderBy(c => c.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Campaign>> GetPaginatedAsync(int page, int pageSize)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Max(pageSize, 12);

            return await InProgressQuery()
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Campaign> GetWithCharityAsync(int id)
        {
            var campaign = await InProgressQuery(includeCharity: true)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (campaign == null)
                throw new InvalidOperationException($"Campaign with ID {id} not found or not in progress.");

            return campaign;
        }

    }
}