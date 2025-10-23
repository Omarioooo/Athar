

using System.Linq.Expressions;

namespace AtharPlatform.Repositories
{
    public class CharityRepository : Repository<Charity>, ICharityRepository
    {
        public CharityRepository(Context context) : base(context) { }

        public override Task<List<Charity>> GetAllAsync()
        {
            return base.GetAllAsync();
        }

        public override Task<Charity?> GetAsync(int id)
        {
            return base.GetAsync(id);
        }

        public override Task<Charity?> GetAsync(Expression<Func<Charity, bool>> expression)
        {
            return base.GetAsync(expression);
        }

        public override Task<bool> AddAsync(Charity entity)
        {
            return base.AddAsync(entity);
        }

        public override Task<bool> Update(Charity entity)
        {
            return base.Update(entity);
        }

        public override Task<bool> DeleteAsync(int id)
        {
            return base.DeleteAsync(id);
        }
    }
}
