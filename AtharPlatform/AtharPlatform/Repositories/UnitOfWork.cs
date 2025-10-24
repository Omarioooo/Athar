

namespace AtharPlatform.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _context;
        public IDonorRepository Donor { get; private set; }
        public ICharityRepository Charity { get; private set; }
        public INotificationRepository Notifications { get; private set; }

        public INotificationTypeRepository NotificationsTypes { get; private set; }

        public UnitOfWork(Context context, IDonorRepository donorRepositroy,
            ICharityRepository charityRepository)
        {
            _context = context;
            Donor = donorRepositroy;
            Charity = charityRepository;
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
