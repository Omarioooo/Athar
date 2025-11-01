namespace AtharPlatform.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IDonorRepository Donor { get; }
        ICharityRepository Charity { get; }
        ICampaignRepository Campaign { get; }
        IContentRepository Content { get; }
        IReactionRepository Reaction { get; }
        INotificationRepository Notifications { get; }
        INotificationTypeRepository NotificationTypes { get; }
        DbSet<Donation> Donations { get; }
        DbSet<CharityDonation> CharityDonations { get; }
        DbSet<CampaignDonation> CampaignDonations { get; }
        DbSet<Follow> Follows { get; }
        DbSet<Subscription> Subscriptions { get; }
        Task SaveAsync();
    }
}
