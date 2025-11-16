

namespace AtharPlatform.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public Context _context { get; }
        public IDonorRepository Donors { get; private set; }
        public ICharityRepository Charities { get; private set; }
        public ICharityRepository Charity => Charities;
        public ICampaignRepository Campaigns { get; private set; }
        public IContentRepository Contents { get; private set; }
        public IReactionRepository Reactions { get; private set; }
        public INotificationRepository Notifications { get; private set; }
        public IFollowRepository Follows { get; private set; }
        public IVendorOfferRepository VendorOffers { get; private set; }
        public IVolunteerApplicationRepository VolunteerApplications { get; private set; }


        // Computed Property
        public DbSet<Donation> Donations => _context.Donations;
        public DbSet<Subscription> Subscriptions => _context.Subscriptions;
        public DbSet<CharityDonation> CharityDonations => _context.CharityDonations;
        public DbSet<CampaignDonation> CampaignDonations => _context.CampaignDonations;


        public UnitOfWork(Context context)
        {
            _context = context;
            Donors = new DonorRepository(_context);
            Charities = new CharityRepository(_context);
            Campaigns = new CampaignRepository(_context);
            Contents = new ContentRepository(_context);
            Reactions = new ReactionRepository(_context);
            Notifications = new NotificationRepository(_context);
            Follows = new FollowRepository(_context);
            VendorOffers = new VendorOfferRepository(_context);
            VolunteerApplications = new VolunteerApplicationRepository(_context);
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
