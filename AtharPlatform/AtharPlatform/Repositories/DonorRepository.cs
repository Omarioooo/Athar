using System.Linq.Expressions;

namespace AtharPlatform.Repositories
{
    public class DonorRepository : Repository<Donor>, IDonorRepository
    {
        public DonorRepository(Context context) : base(context) { }
    }
}
