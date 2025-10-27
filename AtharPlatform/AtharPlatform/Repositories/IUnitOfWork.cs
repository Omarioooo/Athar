namespace AtharPlatform.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IDonorRepository Donor { get; }
        ICharityRepository Charity { get; }
        INotificationRepository Notifications { get; }
        INotificationTypeRepository NotificationTypes { get; }
        DbSet<Subscription> Subscriptions { get; }
        Task SaveAsync();
    }
}
