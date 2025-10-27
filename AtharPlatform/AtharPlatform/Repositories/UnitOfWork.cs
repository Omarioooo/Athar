

namespace AtharPlatform.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _context;
        public IDonorRepository Donor { get; private set; }
        public ICharityRepository Charity { get; private set; }
        public INotificationRepository Notifications { get; private set; }

        public INotificationTypeRepository NotificationTypes { get; private set; }


        // Computed Property
        public DbSet<Subscription> Subscriptions => _context.Subscriptions;

        public UnitOfWork(Context context)
        {
            _context = context;
            Notifications = new NotificationRepository(_context);
            Donor = new DonorRepository(_context);
            Charity = new CharityRepository(_context);
            NotificationTypes = new NotificationTypeRepository(_context);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
