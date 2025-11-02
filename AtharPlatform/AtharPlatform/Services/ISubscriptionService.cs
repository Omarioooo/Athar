using AtharPlatform.DTO;

namespace AtharPlatform.Services
{
    public interface ISubscriptionService
    {
        /// <summary>
        /// Subscribe a donor to a charity and send notification
        /// </summary>
        Task<string> SubscribeAsync(CreateSubscriptionDto model);

        /// <summary>
        /// Update the subscription cost for a donor to a charity and
        /// </summary>
        Task<bool> UpdateSubscriptionAsync(SubscriptionDto model);

        /// <summary>
        /// Unsubscribe a donor from a charity
        /// </summary>
        Task<bool> UnsubscribeAsync(SubscriptionDto model);

        /// <summary>
        /// Check if a donor is subscribed to a charity
        /// </summary>
        Task<bool> IsSubscribedAsync(SubscriptionDto model);
    }
}
