using AtharPlatform.DTOs;

namespace AtharPlatform.Services
{
    public interface IContentService
    {
        Task<Content> CreateContentAsync(CreateContentDTO dto);
        Task<Content?> GetByIdAsync(int id);
        //Task<List<Content>> GetByCampaignIdAsync(int campaignId);
        Task<List<Content>> GetPostsByCampaignAsync(int campaignId);

        Task<List<ContentListDTO>> GetByCampaignIdAsync(int campaignId);
        Task<PagingResponse<ContentListDTO>> GetPagedByCampaignIdAsync(int campaignId, int page, int pageSize);
        Task<Content> UpdateContentAsync(int id, UpdateContentDTO dto);

        Task<bool> DeleteContentAsync(int id);

        Task<PagingResponse<ContentListDTO>> GetPagedByCharityIdAsync(int charityId, int pageNumber, int pageSize);
        Task<PagingResponse<ContentListDTO>> SearchContentsAsync(string keyword, int page = 1, int pageSize = 12);

    }
}
