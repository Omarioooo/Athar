
using AtharPlatform.Models;

namespace AtharPlatform.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public Context _context { get; }
        public IUserAccountRepository Accounts { get; private set; }
        public IDonorRepository Donors { get; private set; }
        public ICharityRepository Charities { get; private set; }
        public ICampaignRepository Campaigns { get; private set; }
        public IContentRepository Contents { get; private set; }
        public IReactionRepository Reactions { get; private set; }
        public INotificationRepository Notifications { get; private set; }
        public IFollowRepository Follows { get; private set; }
        public IVendorOfferRepository VendorOffers { get; }
        public IVolunteerApplicationRepository VolunteerApplications { get; private set; }
        public ICharityVolunteerRepository CharityVolunteers { get; private set; }
        public IMaterialDonationsRepository MaterialDonations { get; private set; }
        public ICharityMaterialDonationsRepository CharityMaterialDonations { get; private set; }
        public IDonationRepository PaymentDonations { get; private set; }
        public ICampaignDonation PaymentCampaignDonations { get; private set; }

        public ICharityVendorOfferRepository CharityVendorOffers { get; private set; }

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
            CharityVolunteers = new CharityVolunteerRepository(_context);
            PaymentDonations = new DonationRepository(_context);
            PaymentCampaignDonations = new CampaignDonationRepository(_context);
            CharityVendorOffers = new CharityVendorOfferRepository(_context);
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