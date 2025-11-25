namespace AtharPlatform.Repositories
{
    public interface ICharityVolunteerRepository
    {
        Task<IEnumerable<CharityVolunteer>> GetAllAsync();
        Task<CharityVolunteer?> GetByIdAsync(int id);
        Task<IEnumerable<CharityVolunteer>> GetByCharityIdAsync(int charityId);
        Task AddAsync(CharityVolunteer entity);
        Task<bool> ExistsAsync(int charityId);
    }
}
