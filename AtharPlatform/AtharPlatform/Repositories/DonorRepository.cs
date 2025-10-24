using System.Linq.Expressions;

namespace AtharPlatform.Repositories
{
    public class DonorRepository : Repository<Donor>, IDonorRepository
    {
        public DonorRepository(Context context) : base(context) { }

        public override Task<List<Donor>> GetAllAsync()
        {
            return base.GetAllAsync();
        }

        public override Task<Donor?> GetAsync(int id)
        {
            return base.GetAsync(id);
        }

        public override Task<Donor?> GetAsync(Expression<Func<Donor, bool>> expression)
        {
            return base.GetAsync(expression);
        }

        public override Task<bool> AddAsync(Donor entity)
        {
            return base.AddAsync(entity);
        }

        public override Task<bool> Update(Donor entity)
        {
            return base.Update(entity);
        }

        public override Task<bool> DeleteAsync(int id)
        {
            return base.DeleteAsync(id);
        }
    }
}
