namespace AtharPlatform.Repositories
{
    internal interface ISubscriptionRepository : IRepository<Subscription>
    {
        Task<List<int>> GetCharitySubscribersAsync(int id);
    }
}