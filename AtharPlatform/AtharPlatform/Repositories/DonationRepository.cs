using System.Linq.Expressions;

namespace AtharPlatform.Repositories
{
    public class DonationRepository : Repository<Donation>,IDonationRepository
    {
        //private readonly Context _context;
        public DonationRepository(Context context) : base(context) { }


        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Set<Donor>().AnyAsync(d => d.Id == id);
        }


        public async Task<bool> AddAsync(Donation entity)
        {
            await _context.Set<Donation>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Donation> GetWithExpressionAsync(Expression<Func<Donation, bool>> expression)
        {
            return await _context.Set<Donation>().FirstOrDefaultAsync(expression);
        }

        public async Task<bool> UpdateAsync(Donation entity)
        {
            _context.Set<Donation>().Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public Task<List<Donation>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Donation> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}

