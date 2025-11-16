
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
    }
}
