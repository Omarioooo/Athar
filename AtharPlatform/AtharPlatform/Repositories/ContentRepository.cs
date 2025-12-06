
using AtharPlatform.DTOs;

namespace AtharPlatform.Repositories
{
    public class ContentRepository : Repository<Content>, IContentRepository
    {
        public ContentRepository(Context context) : base(context)
        {
        }

        public async override Task<Content?> GetAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);

            if (entity == null)
                throw new DbUpdateException("Database erorr");

            return  entity;
        }

        public async Task<List<Content>> GetByCampaignIdAsync(int campaignId)
        {
            return await _context.Contents
                .Where(c => c.CampaignId == campaignId && !c.IsDeleted)
                .OrderByDescending(c => c.CreatedAt) 
                .ToListAsync();
        }

        public async Task<List<Content>> GetPostsByCampaignAsync(int campaignId)
        {
            return await _dbSet
                .Where(c => c.CampaignId == campaignId && !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<PagingResponse<Content>> GetPagedByCampaignIdAsync(int campaignId, int pageNumber, int pageSize)
        {
            var query = _dbSet.Where(c => c.CampaignId == campaignId && !c.IsDeleted);

            var totalItems = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagingResponse<Content>
            {
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = items
            };
        }
        public async Task<Content> GetByIdAsync(int id)
        {
            return await _context.Contents
                                 .Include(c => c.Reactions) // <-- هنا
                                 .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }

    }
}
