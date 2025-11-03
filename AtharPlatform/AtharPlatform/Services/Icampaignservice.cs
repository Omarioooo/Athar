using AtharPlatform.Dtos;
using AtharPlatform.DTOs;
using AtharPlatform.Models.Enum;

namespace AtharPlatform.Services
{
    public interface ICampaignService
    {
        /// <summary>
        /// Retrieves all campaigns, optionally filtering by active (in-progress) status 
        /// and including related charity details.
        /// </summary>
        Task<List<CampaignDto>> GetAllAsync(bool inProgress = true, bool includeCharity = true);

        /// <summary>
        /// Retrieves a single campaign by its ID, optionally including charity details.
        /// </summary>
        Task<CampaignDto> GetAsync(int id, bool inCludeCharity = true);

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
        /// Retrieves campaigns in a paginated format.
        /// </summary>
        Task<List<CampaignDto>> GetPaginatedAsync(int page, int pageSize, bool inCludeCharity = true);

        /// <summary>
        /// Creates a new campaign based on the provided model.
        /// </summary>
        Task<bool> CreateAsync(AddCampaignDto model);

        /// <summary>
        /// Updates an existing campaign with new data.
        /// </summary>
        Task<bool> UpdateAsync(UpdatCampaignDto model);

        /// <summary>
        /// Deletes a campaign by its ID.
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}