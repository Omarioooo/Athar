using AtharPlatform.Dtos;
using AtharPlatform.DTOs;
using AtharPlatform.Models.Enum;

namespace AtharPlatform.Services
{
    public interface ICampaignService
    {

        Task<PaginatedResultDto<CampaignDto>> GetPaginatedOptimizedAsync(
        int page,
        int pageSize,
        CampainStatusEnum? status,
        CampaignCategoryEnum? category,
        string? search,
        bool? isCritical,
        double? minGoalAmount,
        double? maxGoalAmount,
        DateTime? startDateFrom,
        DateTime? startDateTo,
        int? charityId
    );

        /// <summary>
        /// Get the total number of campaigns with optional filters
        /// </summary>
        Task<int> GetCountOfCampaignsAsync(
            CampainStatusEnum? status = null,
            CampaignCategoryEnum? category = null,
            string? search = null,
            bool? isCritical = null,
            double? minGoalAmount = null,
            double? maxGoalAmount = null,
            DateTime? startDateFrom = null,
            DateTime? startDateTo = null,
            int? charityId = null);

        /// <summary>
        /// Retrieves all campaigns, optionally filtering by active (in-progress) status 
        /// and including related charity details.
        /// </summary>
        Task<List<CampaignDto>> GetAllAsync(bool inProgress = true, bool includeCharity = true);

        /// <summary>
        /// Retrieves a single campaign by its ID, optionally including charity details.
        /// </summary>
        Task<CampaignDto> GetAsync(int id, bool inProgress = true, bool inCludeCharity = true);

        /// <summary>
        /// Retrieves all campaigns that belong to a specific category type.
        /// </summary>
        Task<List<CampaignDto>> GetByTypeAsync(CampaignCategoryEnum type, bool inCludeCharity = true);

        /// <summary>
        /// Returns a list of all available campaign category types.
        /// </summary>
        Task<List<string>> GetAllTypesAsync();

        /// <summary>
        /// Searches for campaigns that match a specific keyword.
        /// </summary>
        Task<List<CampaignDto>> SearchAsync(string keyword, bool inCludeCharity = true);

        /// <summary>
        /// Retrieves campaigns in a paginated format with optional filters.
        /// </summary>
        Task<List<CampaignDto>> GetPaginatedAsync(
            int page,
            int pageSize,
            CampainStatusEnum? status = null,
            CampaignCategoryEnum? category = null,
            string? search = null,
            bool? isCritical = null,
            double? minGoalAmount = null,
            double? maxGoalAmount = null,
            DateTime? startDateFrom = null,
            DateTime? startDateTo = null,
            int? charityId = null,
            bool inCludeCharity = true);

        /// <summary>
        /// Creates a new campaign based on the provided model.
        /// </summary>
        Task<bool> CreateAsync(AddCampaignDto model);

        /// <summary>
        /// Updates an existing campaign with new data.
        /// </summary>
        Task<Campaign> UpdateAsync(UpdatCampaignDto model);

        /// <summary>
        /// Deletes a campaign by its ID.
        /// </summary>
        Task<bool> DeleteAsync(int id);

        Task<List<CampaignDto>> GetCampaignsByCharityIdAsync(int charityId);

        Task<int> GetCountOfCampaignsByCharityIdAsync(int CharityId);
    }
}