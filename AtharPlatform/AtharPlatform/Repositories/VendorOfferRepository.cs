using System;
using System.Linq.Expressions;
using AtharPlatform.Models;

namespace AtharPlatform.Repositories
{
    public class VendorOfferRepository : Repository<VendorOffers>, IVendorOfferRepository
    {
        public VendorOfferRepository(Context context) : base(context) { }

        public async Task<VendorOffers> GetAsync(int id)
        {
            return await _context.VendorForms
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<List<VendorOffers>> GetByCampaignAsync(int charityVendorOfferId)
        {
            return await _context.VendorForms
                .Where(v => v.CharityVendorOfferId == charityVendorOfferId)
                .ToListAsync();
        }
        public async Task<List<VendorOffers>> GetByCharityIdAsync(int charityId)
        {
            return await _context.VendorForms
                                 .Where(c => c.CharityVendorOfferId == charityId)
                                 .ToListAsync();
        }

        public async Task AddAsync(CharityVendorOffer entity)
        {
            await _context.CharityVendorOffers.AddAsync(entity);
        }
        public async Task<CharityVendorOffer?> GetSlotByCharityIdAsync(int charityId)
        {
            return await _context.CharityVendorOffers
                .FirstOrDefaultAsync(c => c.CharityId == charityId);
        }

       
    }
}