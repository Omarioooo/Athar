namespace AtharPlatform.Repositories
{
    public interface IDonorRepository : IRepository<Donor>
    {
        /// <summary>
        /// Get all charities id the user follows
        /// </summary>
        Task<List<int>> GetFollowsAsync(int id);

        /// <summary>
        /// Get all charities id the user subsscritions
        /// </summary>
        Task<List<int>> GetSubscriptionsAsync(int id);

    }
}
