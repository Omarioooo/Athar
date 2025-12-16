namespace AtharPlatform.Repositories
{
    public interface IDonationRepository: IRepository<Donation>
    {
        /// <summary>
        /// Get all donations made by a specific donor
        /// </summary>
        Task<List<Donation>> GetDonationsByDonorIdAsync(int donorId);
    }
}
