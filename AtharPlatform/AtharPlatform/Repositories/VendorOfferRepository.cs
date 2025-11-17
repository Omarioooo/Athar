
using System;
using System.Linq.Expressions;
using AtharPlatform.Models;

namespace AtharPlatform.Repositories
{
    public class VendorOfferRepository : Repository<VendorOffers>, IVendorOfferRepository
    {
        public VendorOfferRepository(Context context) : base(context) { }

        public async override Task<List<VendorOffers>> GetAllAsync()
        {
            return await _context.VendorForms
                .Include(v => v.CharityVendorOffer)
                .ToListAsync();
        }

        public Task<List<VendorOffers>> GetByCampaignAsync(int charityVendorOfferId)
        {
            throw new NotImplementedException();
        }
    }
}