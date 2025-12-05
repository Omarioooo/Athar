namespace AtharPlatform.Repositories
{
    public interface ICharityVendorOfferRepository
    {
        Task<IEnumerable<CharityVendorOffer>> GetAllAsync();
        Task<CharityVendorOffer?> GetByIdAsync(int id);
        Task<IEnumerable<CharityVendorOffer>> GetByCharityIdAsync(int charityId);
        Task AddAsync(CharityVendorOffer entity);
        Task<bool> ExistsAsync(int charityId);
        Task<CharityVendorOffer?> GetSlotByCharityIdAsync(int charityId);

    }
}
