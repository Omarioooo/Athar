using System.Linq.Expressions;

namespace AtharPlatform.Repositories
{
    public class DonationRepository : Repository<Donation>, IDonationRepository
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
            return _context.Set<Donation>()
                .Include(d => d.CampaignDonations)
                .ThenInclude(cd => cd.Campaign)
                .ToListAsync();
        }

        public Task<Donation> GetAsync(int id)
        {
            return _context.Set<Donation>()
                .Include(d => d.CampaignDonations)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Donation>> GetDonationsByDonorIdAsync(int donorId)
        {
            return await _context.Set<Donation>()
                .Where(d => d.DonorId == donorId)
                .Include(d => d.CampaignDonations)
                .ThenInclude(cd => cd.Campaign)
                .Include(d => d.CharityDonations)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }
    }
}

