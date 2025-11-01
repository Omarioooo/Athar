namespace AtharPlatform.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IDonorRepository Donor { get; }
        ICharityRepository Charity { get; }
        INotificationRepository Notifications { get; }
        INotificationTypeRepository NotificationsTypes { get; }

        ICampaignRepository Campaign { get; }
        IVendorOfferRepository VendorOffers { get; }
        IVolunteerApplicationRepository VolunteerApplications { get; }

        Task SaveAsync();
    }
}
