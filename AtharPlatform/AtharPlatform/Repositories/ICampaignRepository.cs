using AtharPlatform.Models.Enum;

namespace AtharPlatform.Repositories
{
    public interface ICampaignRepository : IRepository<Campaign>
    {
        /// <summary>
        /// Get a campaign by its ID.
        /// </summary>
        Task<Campaign> GetAsync(int id, bool inProgress = true, bool includeCharity = true);

        /// <summary>
        /// Get all campaigns.
        /// </summary>
        Task<List<Campaign>> GetAllAsync(bool includeCharity = true);

        /// <summary>
        /// Get all campaigns that are currently in progress.
        /// </summary>
        Task<List<Campaign>> GetAllInProgressAsync(bool includeCharity = true);

        /// <summary>
        /// Get all campaigns by category type.
        /// </summary>
        Task<List<Campaign>> GetAllByTypeAsync(CampaignCategoryEnum type, bool includeCharity = true);

        /// <summary>
        /// Get all in-progress campaigns by category type.
        /// </summary>
        Task<List<Campaign>> GetAllInProgressByTypeAsync(CampaignCategoryEnum type, bool includeCharity = true);

        /// <summary>
        /// Search campaigns by keyword.
        /// </summary>
        Task<List<Campaign>> Search(string keyword, bool includeCharity = true);

        /// <summary>
        /// Get campaigns by query with pagination.
        /// </summary>
        Task<List<Campaign>> GetPageAsync(string? query, int page, int pageSize, bool includeCharity = true);

        /// <summary>
        /// Get paginated list of campaigns.
        /// </summary>
        Task<List<Campaign>> GetPaginatedAsync(int page, int pageSize, bool includeCharity = true);

        /// <summary>
        /// Get queryable for custom filtering.
        /// </summary>
        IQueryable<Campaign> GetQueryable();


        Task<List<Campaign>> GetByCharityIdAsync(int charityId);

    }

}