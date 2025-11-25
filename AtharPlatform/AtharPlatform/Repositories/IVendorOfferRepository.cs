using AtharPlatform.Models;

namespace AtharPlatform.Repositories
{
    public interface IVendorOfferRepository : IRepository<VendorOffers>
    {
        public Task<List<VendorOffers>> GetByCampaignAsync(int charityVendorOfferId);
        Task<IEnumerable<CharityVendorOffer>> GetByCharityIdAsync(int charityId);
        Task AddAsync(CharityVendorOffer entity);
    }
}
