using AtharPlatform.Models.Enum;

namespace AtharPlatform.Repositories
{
    public class CampaignRepository : Repository<Campaign>, ICampaignRepository
    {
        public CampaignRepository(Context context) : base(context) { }

        #region Query Helpers

        private IQueryable<Campaign> IncludeCharityQuery(bool includeCharity = false)
        {
            var query = _dbSet.AsQueryable();

            if (includeCharity)
                query = query.Include(c => c.Charity);

            return query;
        }

        private IQueryable<Campaign> InProgressQuery(bool includeCharity = false)
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


        public async Task<Campaign> GetAsync(int id, bool includeCharity = false)
        {
            var campaign = await InProgressQuery(includeCharity: includeCharity)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (campaign == null)
                throw new InvalidOperationException($"Campaign with ID {id} not found or not in progress.");

            return campaign;
        }

        public async Task<List<Campaign>> GetAllAsync(bool includeCharity = false)
        {
            return await IncludeCharityQuery(includeCharity: includeCharity).ToListAsync();
        }

        public async Task<List<Campaign>> GetAllInProgressAsync(bool includeCharity = false)
        {
            return await InProgressQuery(includeCharity: includeCharity).ToListAsync();
        }

        public async Task<List<Campaign>> GetAllByTypeAsync(CampaignCategoryEnum type, bool includeCharity = false)
        {
            return await IncludeCharityQuery(includeCharity: includeCharity)
                .Where(c => c.Category == type)
                .ToListAsync();
        }

        public async Task<List<Campaign>> GetAllInProgressByTypeAsync(CampaignCategoryEnum type, bool includeCharity = false)
        {
            return await InProgressQuery(includeCharity: includeCharity)
                .Where(c => c.Category == type)
                .ToListAsync();
        }

        public async Task<List<Campaign>> Search(string keyword, bool includeCharity = false)
        {
            return await ApplySearchFilter(InProgressQuery(includeCharity: includeCharity), keyword)
                .ToListAsync();
        }

        public async Task<List<Campaign>> GetPageAsync(string? query, int page, int pageSize, bool includeCharity = false)
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

        public async Task<List<Campaign>> GetPaginatedAsync(int page, int pageSize, bool includeCharity = false)
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