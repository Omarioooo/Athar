namespace AtharPlatform.Repositories
{
    public interface ICharityRepository : IRepository<Charity>
    {
        /// <summary>
        /// Get the charity based on one of it's campagins
        /// </summary>
        Task<Charity> GetCharityByCampaignAsync(int campaignId);
        Task<List<Charity>> GetPageAsync(
            string? query, 
            int page, 
            int pageSize,
            bool? isActive = null,
            bool? isScraped = null,
            bool? hasExternalWebsite = null);
        Task<int> CountAsync(
            string? query,
            bool? isActive = null,
            bool? isScraped = null,
            bool? hasExternalWebsite = null);
        Task<Charity?> GetWithCampaignsAsync(int id);
        Task BulkImportAsync(IEnumerable<Charity> charities);

        Task<Charity> GetCharityFullProfileAsync(int id);
        Task<Charity> GetCharityViewAsync(int id);
    }
}