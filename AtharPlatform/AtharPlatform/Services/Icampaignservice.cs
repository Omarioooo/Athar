using AtharPlatform.Dtos;
using AtharPlatform.DTOs;
using AtharPlatform.Models.Enum;

public interface ICampaignService
{
    Task<IEnumerable<CampaignDto>> GetAllAsyncforadmin();
    Task<IEnumerable<CampaignDto>> GetAllAsyncforusers();
    Task<CampaignDto> GetByIdAsync(int id);
    Task<CampaignDto> GetByIdAsynctousers(int id);
    Task<IEnumerable<CampaignDto>> GetByTypeAsync(CampaignCategoryEnum type);
    Task<IEnumerable<CampaignDto>> GetByTypeAsynctousers(CampaignCategoryEnum type);
    Task<IEnumerable<object>> GetAllTypesAsync();
    Task<IEnumerable<CampaignDto>> SearchAsync(string keyword);
    Task<IEnumerable<CampaignDto>> GetPaginatedAsync(int page, int pageSize);
    Task<CampaignDto> CreateAsync(AddCampaignDto model);
    Task<CampaignDto> UpdateAsync(UpdatCampaignDto model);
    Task DeleteAsync(int id);
}
