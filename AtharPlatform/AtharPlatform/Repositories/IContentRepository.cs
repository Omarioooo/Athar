using AtharPlatform.DTOs;

namespace AtharPlatform.Repositories
{
    public interface IContentRepository : IRepository<Content>
    {
        Task<List<Content>> GetByCampaignIdAsync(int campaignId);
        Task<List<Content>> GetPostsByCampaignAsync(int campaignId);
        Task<PagingResponse<Content>> GetPagedByCampaignIdAsync(int campaignId, int pageNumber, int pageSize);

    }
}