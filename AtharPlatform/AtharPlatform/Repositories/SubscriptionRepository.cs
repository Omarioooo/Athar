namespace AtharPlatform.Repositories
{
    public class SubscriptionRepository : Repository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(Context context) : base(context) { }

        public async Task<List<int>> GetCharitySubscribersAsync(int id)
        {
            var donorIds = await _dbSet
                .Where(f => f.CharityId == id)
                .Select(f => f.DonorId)
                .ToListAsync();

            return donorIds;
        }
    }
}