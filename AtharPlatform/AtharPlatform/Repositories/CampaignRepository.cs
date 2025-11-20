using AtharPlatform.Models.Enum;

namespace AtharPlatform.Repositories
{
    public class CampaignRepository : Repository<Campaign>, ICampaignRepository
    {
        public CampaignRepository(Context context) : base(context) { }

        #region Query Helpers

        public IQueryable<Campaign> GetQueryable()
        {
            return _dbSet.AsQueryable();
        }

        private IQueryable<Campaign> IncludeCharityQuery(bool includeCharity = true)
        {
            var query = _dbSet.AsQueryable();

            if (includeCharity)
                query = query.Include(c => c.Charity);

            return query;
        }

        private IQueryable<Campaign> InProgressQuery(bool includeCharity = true)
        {
            return IncludeCharityQuery(includeCharity)
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


        public async Task<Campaign> GetAsync(int id, bool inProgress = true, bool includeCharity = true)
        {
            var campaign = inProgress
               ? await InProgressQuery(includeCharity: includeCharity).FirstOrDefaultAsync(c => c.Id == id)
               : await IncludeCharityQuery(includeCharity: includeCharity).FirstOrDefaultAsync(c => c.Id == id);

            if (campaign == null)
                throw new KeyNotFoundException($"Campaign with ID {id} not found or not in progress.");

            return campaign;
        }

        public async Task<List<Campaign>> GetAllAsync(bool includeCharity = true)
        {
            return await IncludeCharityQuery(includeCharity: includeCharity).ToListAsync();
        }

        public async Task<List<Campaign>> GetAllInProgressAsync(bool includeCharity = true)
        {
            return await InProgressQuery(includeCharity: includeCharity).ToListAsync();
        }

        public async Task<List<Campaign>> GetAllByTypeAsync(CampaignCategoryEnum type, bool includeCharity = true)
        {
            return await IncludeCharityQuery(includeCharity: includeCharity)
                .Where(c => c.Category == type)
                .ToListAsync();
        }

        public async Task<List<Campaign>> GetAllInProgressByTypeAsync(CampaignCategoryEnum type, bool includeCharity = true)
        {
            return await InProgressQuery(includeCharity: includeCharity)
                .Where(c => c.Category == type)
                .ToListAsync();
        }

        public async Task<List<Campaign>> Search(string keyword, bool includeCharity = true)
        {
            return await ApplySearchFilter(InProgressQuery(includeCharity: includeCharity), keyword)
                .ToListAsync();
        }

        public async Task<List<Campaign>> GetPageAsync(string? query, int page, int pageSize, bool includeCharity = true)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Max(pageSize, 12);

            var q = ApplySearchFilter(IncludeCharityQuery(includeCharity: includeCharity), query);

            return await q
                .OrderBy(c => c.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Campaign>> GetPaginatedAsync(int page, int pageSize, bool includeCharity = true)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Max(pageSize, 12);

            return await InProgressQuery(includeCharity: includeCharity)
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}