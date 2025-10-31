
using System;
using System.Linq.Expressions;
using AtharPlatform.Models;

namespace AtharPlatform.Repositories
{
    public class VendorOfferRepository : Repository<VendorOffers>,IVendorOfferRepository
    {
    
        public VendorOfferRepository(Context context) : base(context) { }

        public override Task<bool> AddAsync(VendorOffers entity)
        {
            return base.AddAsync(entity);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return base.DeleteAsync(id);
        }

        public Task<List<VendorOffers>> GetAllAsync()
        {
            return base.GetAllAsync();
        }

        public Task<VendorOffers> GetAsync(int id)
        {
            return base.GetAsync(id);
        }

        public Task<VendorOffers> GetAsync(Expression<Func<VendorOffers, bool>> expression)
        {
            return base.GetAsync(expression);
        }

        public async Task<List<VendorOffers>> GetByCampaignAsync(int charityVendorOfferId)
        {
            return await _context.VendorForms
            .Where(v => v.CharityVendorOfferId == charityVendorOfferId)
            .Include(v => v.CharityVendorOffer)
            .ToListAsync();
        }

        public Task<bool> Update(VendorOffers entity)
        {
            return base.Update(entity);
        }
    }
}
