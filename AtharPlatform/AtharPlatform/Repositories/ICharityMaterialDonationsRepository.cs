namespace AtharPlatform.Repositories
{
    public interface ICharityMaterialDonationsRepository
    {
        Task<IEnumerable<CharityMaterialDonation>> GetAllAsync();
        Task<CharityMaterialDonation?> GetByIdAsync(int id);
        Task<IEnumerable<CharityMaterialDonation>> GetByCharityIdAsync(int charityId);
        Task AddAsync(CharityMaterialDonation entity);
        Task<bool> ExistsAsync(int charityId);
        Task<CharityMaterialDonation?> GetSlotByCharityIdAsync(int charityId);
    }
}
