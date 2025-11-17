namespace AtharPlatform.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Context _context { get; }
        IDonorRepository Donors { get; }
        ICharityRepository Charities { get; }
        ICampaignRepository Campaigns { get; }
        IContentRepository Contents { get; }
        IReactionRepository Reactions { get; }
        INotificationRepository Notifications { get; }
        IFollowRepository Follows { get; }
        DbSet<Donation> Donations { get; }
        DbSet<CharityDonation> CharityDonations { get; }
        DbSet<CampaignDonation> CampaignDonations { get; }
        DbSet<Subscription> Subscriptions { get; }
        IVendorOfferRepository VendorOffers { get; }
        IVolunteerApplicationRepository VolunteerApplications { get; }

        IDonationRepository PaymentDonations { get; }
        ICampaignDonation PaymentCampaignDonations { get; }

        Task SaveAsync();
    }       
}
