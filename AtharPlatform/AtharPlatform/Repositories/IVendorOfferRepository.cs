using AtharPlatform.Models;

namespace AtharPlatform.Repositories
{
    public interface IVendorOfferRepository:IRepository<VendorOffers>
    {
        public Task<List<VendorOffers>> GetByCampaignAsync(int charityVendorOfferId);
    }
}
