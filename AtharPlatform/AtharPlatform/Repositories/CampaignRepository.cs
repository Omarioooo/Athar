using AtharPlatform.Models;
using AtharPlatform.Models.Enum;

namespace AtharPlatform.Repositories
{
    public class CampaignRepository : Repository<Campaign>, ICampaignRepository
    {
        public CampaignRepository(Context context) : base(context)
        {
        }

        public async Task<List<Campaign>> Getallforusers()
        {
            return await _context.Campaigns
                .Include(c => c.Charity)
                .Where(c => c.Charity != null && c.Charity.IsActive && c.Status == CampainStatusEnum.inProgress)
                .ToListAsync();
        }

        public async Task<List<Campaign>> GetByType(CampaignCategoryEnum type)
        {
            return await _context.Campaigns
                .Include(c => c.Charity)
                .Where(c => c.Category == type)
                .ToListAsync();
        }

        public async Task<List<Campaign>> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<Campaign>();

            var term = keyword.Trim();
            return await _context.Campaigns
                .Include(c => c.Charity)
                .Where(c => c.Title.Contains(term) || c.Description.Contains(term) || (c.Charity != null && c.Charity.Name.Contains(term)))
                .ToListAsync();
        }

        public async Task<List<Campaign>> GetPaginated(int page, int pageSize)
        {
            if (page <= 0 || pageSize <= 0) return new List<Campaign>();
            return await _context.Campaigns
                .Include(c => c.Charity)
                .OrderBy(c => c.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public void Update(Campaign entity)
        {
            _context.Campaigns.Update(entity);
        }

        public async Task<Campaign?> GetAsyncTousers(int id)
        {
            return await _context.Campaigns
                .Include(c => c.Charity)
                .Where(c => c.Id == id && c.Charity != null && c.Charity.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Campaign>> GetByTypetousers(CampaignCategoryEnum type)
        {
            return await _context.Campaigns
                .Include(c => c.Charity)
                .Where(c => c.Category == type && c.Charity != null && c.Charity.IsActive)
                .ToListAsync();
        }
    }
}
