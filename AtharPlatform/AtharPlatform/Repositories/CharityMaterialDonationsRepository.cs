using AtharPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace AtharPlatform.Repositories
{
    public class CharityMaterialDonationsRepository : ICharityMaterialDonationsRepository
    {
        private readonly Context _context;

        public CharityMaterialDonationsRepository(Context context)
        {
            _context = context;
        }

        public async Task AddAsync(CharityMaterialDonation entity)
        {
            await _context.CharityMaterialDonations.AddAsync(entity);
        }

        public async Task<bool> ExistsAsync(int charityId)
        {
            return await _context.CharityMaterialDonations
                .AnyAsync(c => c.CharityId == charityId);
        }

        public async Task<IEnumerable<CharityMaterialDonation>> GetAllAsync()
        {
            return await _context.CharityMaterialDonations
                .Include(c => c.MaterialDonations)
                .ToListAsync();
        }

        public async Task<IEnumerable<CharityMaterialDonation>> GetByCharityIdAsync(int charityId)
        {
            return await _context.CharityMaterialDonations
                .Where(c => c.CharityId == charityId)
                .Include(c => c.MaterialDonations)
                .ToListAsync();
        }

        public async Task<CharityMaterialDonation?> GetByIdAsync(int id)
        {
            return await _context.CharityMaterialDonations
                .Include(c => c.MaterialDonations)
                .FirstOrDefaultAsync(c => c.CharityId == id);
        }
    }
}