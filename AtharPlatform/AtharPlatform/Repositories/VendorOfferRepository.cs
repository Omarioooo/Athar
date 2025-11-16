
using System;
using AtharPlatform.Models;

namespace AtharPlatform.Repositories
{
    public class VendorOfferRepository : Repository<VendorOffers>, IVendorOfferRepository
    {
        public VendorOfferRepository(Context context) : base(context) { }

        public async Task<List<VendorOffers>> GetByCampaignAsync(int charityVendorOfferId)
        {
            return await _context.VendorForms
            .Where(v => v.CharityVendorOfferId == charityVendorOfferId)
            .Include(v => v.CharityVendorOffer)
            .ToListAsync();
        }
    }
}
