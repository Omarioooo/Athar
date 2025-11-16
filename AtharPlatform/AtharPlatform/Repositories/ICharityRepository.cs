namespace AtharPlatform.Repositories
{
    public interface ICharityRepository : IRepository<Charity>
    {
        /// <summary>
        /// Get all subscribers id of a charity
        /// </summary>
        Task<List<int>> GetCharitySubscribersAsync(int id);

        /// <summary>
        /// Get the charity based on one of it's campagins
        /// </summary>
        Task<Charity> GetCharityByCampaignAsync(int campaignId);

        // Extended operations used by controllers
        Task<int> CountAsync(string? query);
        Task<List<Charity>> GetPageAsync(string? query, int page, int pageSize);
        Task<Charity?> GetWithCampaignsAsync(int id);
        Task BulkImportAsync(IEnumerable<Charity> items);
    }
}