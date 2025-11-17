
using AtharPlatform.DTO;
using AtharPlatform.Models;
using AtharPlatform.Models.Enum;
using AtharPlatform.Models.Enums;
using AtharPlatform.Repositories;

namespace AtharPlatform.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;
        private readonly IAccountContextService _accountContextService;
        private readonly INotificationService _notificationService;

        public SubscriptionService(IUnitOfWork unitOfWork,
                 IPaymentService paymentService,
                 IAccountContextService accountContextService,
                 INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            _accountContextService = accountContextService;
            _notificationService = notificationService;
        }


        public async Task<string> SubscribeAsync(CreateSubscriptionDto model)
        {
            // Check the donor
            var donor = await _unitOfWork.Donors.GetAsync(model.DonorId);
            if (donor == null)
                throw new Exception($"Donor with id {model.DonorId} not found");

            // Check the charity
            var charity = await _unitOfWork.Charities.GetAsync(model.CharityId);
            if (charity == null)
                throw new Exception($"Charity with id {model.CharityId} not found");

            // Check the state of the Status of the charity
            if (charity.Status != CharityStatusEnum.Approved)
                throw new Exception($"Charity is not approved yet.");

            // Check if already subscribed
            if (!await IsSubscribedAsync(new SubscriptionDto() { DonorID = model.DonorId, CharityID = model.CharityId }))
                throw new Exception("You are already Subscribed to this charity");

            // Create the subscription entity
            var subscription = new Subscription
            {
                DonorId = model.DonorId,
                CharityId = model.CharityId,
                Amount = model.Amount,
                Frequency = model.Frequency,
                StartDate = DateTime.UtcNow,
                NextPaymentDate = DateTime.UtcNow.AddMonths(1)
            };

            // Subscription payment
            var paymentUrl = await _paymentService.CreatePaymentAsync(new CreatePaymentDto
            {
                DonorFirstName = donor.FirstName,
                DonorLastName = donor.LastName,
                DonorEmail = donor.Account.Email,
                Amount = model.Amount,
                MerchantOrderId = $"subscription_{subscription.Id}_{Guid.NewGuid()}"
            });

            if (paymentUrl == null)
                throw new Exception("subscription payment field...");

            // save the subscription to database
            _unitOfWork.Subscriptions.Add(subscription);
            await _unitOfWork.SaveAsync();

            // Notify the charity
            await _notificationService.SendNotificationAsync(model.DonorId,
                [model.CharityId], NotificationsTypeEnum.NewSubscriber);

            return "";

            //return paymentUrl;
        }

        public async Task<bool> UnsubscribeAsync(SubscriptionDto model)
        {
            // get the subscription
            var subscription = _unitOfWork
                .Subscriptions
                .FirstOrDefault(S => S.DonorId == model.DonorID && S.CharityId == model.CharityID);

            if (subscription == null)
                throw new Exception("There is no subscription");


            // Remove and save
            _unitOfWork.Subscriptions.Remove(subscription);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<bool> UpdateSubscriptionAsync(SubscriptionDto model)
        {
            // get the subscription
            var subscription = _unitOfWork
                .Subscriptions
                .FirstOrDefault(S => S.DonorId == model.DonorID && S.CharityId == model.CharityID);

            if (subscription == null)
                throw new Exception("There is no subscription");

            // Do updates
            subscription.Amount = model.Amount;

            // Save updates
            _unitOfWork.Subscriptions.Update(subscription);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<bool> IsSubscribedAsync(SubscriptionDto model)
        {
            return await _unitOfWork
                .Subscriptions
                .AnyAsync(s => s.DonorId == model.DonorID && s.CharityId == model.CharityID);
        }

    }
}
