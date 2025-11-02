namespace AtharPlatform.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IDonorRepository Donors { get; }
        ICharityRepository Charities { get; }
        ICampaignRepository Campaigns { get; }
        IContentRepository Contents { get; }
        IReactionRepository Reactions { get; }
        INotificationRepository Notifications { get; }
        INotificationTypeRepository NotificationTypes { get; }
        IFollowRepository Follows { get; }
        DbSet<Donation> Donations { get; }
        DbSet<CharityDonation> CharityDonations { get; }
        DbSet<CampaignDonation> CampaignDonations { get; }
        DbSet<Subscription> Subscriptions { get; }
        Task SaveAsync();
    }
}
