using AtharPlatform.Models.Enums;
using System.Linq.Expressions;

namespace AtharPlatform.Repositories
{
    public class DonorRepository : Repository<Donor>, IDonorRepository
    {
        public DonorRepository(Context context) : base(context) { }


        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Set<Donor>().AnyAsync(d => d.Id == id);
        }
        public async override Task<Donor> GetAsync(int id)
        {
            var donor = await _dbSet.Include(d => d.Account)
                .Where(d => d.Role == RolesEnum.Donor)
                 .FirstOrDefaultAsync(d => d.Id == id);

            if (donor == null)
                throw new KeyNotFoundException($"Donor with id {id} not found");

            return donor;
        }

        public async override Task<List<Donor>> GetAllAsync()
        {
            var donors = await _dbSet.Include(d => d.Account)
                .Where(d => d.Role == RolesEnum.Donor)
                .ToListAsync();

            if (donors == null)
                throw new KeyNotFoundException($"Donors not found");

            return donors;
        }

        public async override Task<Donor> GetWithExpressionAsync(Expression<Func<Donor, bool>> expression)
        {
            var donors = await _dbSet.Include(d => d.Account)
                .Where(d => d.Role == RolesEnum.Donor)
                .FirstOrDefaultAsync(expression);

            if (donors == null)
                throw new KeyNotFoundException($"Donor not found");

            return donors;
        }

        public async Task<List<int>> GetAllAdminsIdsAsync()
        {
            var admins = await _dbSet.Include(d => d.Account)
                .Where(d => d.Role == RolesEnum.Admin)
                .Select(a => a.Id)
                .ToListAsync();

            if (admins == null)
                throw new KeyNotFoundException($"Admins not found");

            return admins;
        }

        public async Task<Donor> GetDonorFullProfileAsync(int id)
        {
            var donor = await _dbSet
                .Include(d => d.Account)
                .Include(d => d.Donations)
                 .ThenInclude(cd => cd.Donation)
                .Include(d => d.Follows)
                .Where(d => d.Role == RolesEnum.Donor)
                .FirstOrDefaultAsync(d => d.Id == id);


            return donor;
        }
    }
}
