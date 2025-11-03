namespace AtharPlatform.Repositories
{
    public interface ICharityRepository : IRepository<Charity>
    {
        /// <summary>
        /// Get the charity based on one of it's campagins
        /// </summary>
        Task<Charity> GetCharityByCampaignAsync(int campaignId);
        Task<List<Charity>> GetPageAsync(string? query, int page, int pageSize);
        Task<int> CountAsync(string? query);
        Task<Charity?> GetWithCampaignsAsync(int id);
        Task BulkImportAsync(IEnumerable<Charity> charities);
    }
}