namespace AtharPlatform.Repositories
{
    public interface IDonorRepository : IRepository<Donor>
    {
        Task<List<int>> GetAllAdminsIdsAsync();
        Task<bool> ExistsAsync(int id);
    }
}