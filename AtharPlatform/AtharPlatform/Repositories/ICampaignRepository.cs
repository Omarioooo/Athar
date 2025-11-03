using AtharPlatform.Models.Enum;

namespace AtharPlatform.Repositories
{
    public interface ICampaignRepository : IRepository<Campaign>
    {
        /// <summary>
        /// Retrieves all campaigns with related charities.
        /// </summary>
        Task<List<Campaign>> GetAllWithCharitiesAsync();

        /// <summary>
        /// Retrieves all active (in-progress) campaigns with related charities.
        /// </summary>
        Task<List<Campaign>> GetAllInProgressWithCharitiesAsync();

        /// <summary>
        /// Retrieves all campaigns of a specific type with related charities.
        /// </summary>
        Task<List<Campaign>> GetAllByTypeAsync(CampaignCategoryEnum type);

        /// <summary>
        /// Retrieves all in-progress campaigns of a specific type.
        /// </summary>
        Task<List<Campaign>> GetAllInProgressByTypeAsync(CampaignCategoryEnum type);

        /// <summary>
        /// Searches for campaigns matching the given keyword among active campaigns.
        /// </summary>
        Task<List<Campaign>> Search(string keyword);

        /// <summary>
        /// Retrieves a paginated list of campaigns, optionally filtered by a query string.
        /// </summary>
        Task<List<Campaign>> GetPageAsync(string? query, int page, int pageSize);

        /// <summary>
        /// Retrieves a paginated list of in-progress campaigns.
        /// </summary>
        Task<List<Campaign>> GetPaginatedAsync(int page, int pageSize);

        /// <summary>
        /// Retrieves a single campaign (in-progress only) with its related charity.
        /// </summary>
        Task<Campaign> GetWithCharityAsync(int id);
    }
}