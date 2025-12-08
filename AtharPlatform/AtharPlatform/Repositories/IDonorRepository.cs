namespace AtharPlatform.Repositories
{
    public interface IDonorRepository : IRepository<Donor>
    {
        Task<Donor> getDonorWithId(int id);
        Task<List<int>> GetAllAdminsIdsAsync();
        Task<bool> ExistsAsync(int id);
        Task<Donor> GetDonorFullProfileAsync(int id);
        Task DeleteDonorAsync(int id);
        Task DeleteDonorAsync(Donor donor);

    }
}