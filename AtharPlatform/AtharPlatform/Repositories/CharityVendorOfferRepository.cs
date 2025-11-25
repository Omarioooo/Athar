namespace AtharPlatform.Repositories
{
    public class CharityVendorOfferRepository : ICharityVendorOfferRepository
    {
        private readonly Context _context;

        public CharityVendorOfferRepository(Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CharityVendorOffer>> GetAllAsync()
        {
            return await _context.CharityVendorOffers.ToListAsync();
        }

        public async Task<CharityVendorOffer?> GetByIdAsync(int id)
        {
            return await _context.CharityVendorOffers.FindAsync(id);
        }

        public async Task<IEnumerable<CharityVendorOffer>> GetByCharityIdAsync(int charityId)
        {
            return await _context.CharityVendorOffers
                                 .Where(c => c.CharityId == charityId)
                                 .ToListAsync();
        }

        public async Task AddAsync(CharityVendorOffer entity)
        {
            await _context.CharityVendorOffers.AddAsync(entity);
        }

        public async Task<bool> ExistsAsync(int charityId)
        {
            return await _context.CharityVendorOffers
                                 .AnyAsync(c => c.CharityId == charityId);
        }
    }
}
