namespace AtharPlatform.Repositories
{
    public class ContentRepository : Repository<Content>, IContentRepository
    {
        public ContentRepository(Context context) : base(context)
        {
        }
    }
}
