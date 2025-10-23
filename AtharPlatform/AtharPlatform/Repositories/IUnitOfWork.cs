namespace AtharPlatform.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IDonorRepository Donor { get; }
        ICharityRepository Charity { get; }
        INotificationRepository Notifications { get; }
        INotificationTypeRepository NotificationsTypes { get; }
        Task SaveAsync();
    }
}
