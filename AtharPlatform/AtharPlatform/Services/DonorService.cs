using AtharPlatform.DTOs;
using AtharPlatform.Models;
using AtharPlatform.Models.Enum;
using AtharPlatform.Models.Enums;
using AtharPlatform.Repositories;
using System.Drawing;

namespace AtharPlatform.Services
{
    public class DonorService : IDonorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountContextService _accountContextService;
        private readonly INotificationService _notificationService;
        private readonly IAccountService _accountService;
        private readonly IFollowService _followService;

        public DonorService(IUnitOfWork unitOfWork, ISubscriptionService subscriptionService,
            INotificationService notificationService, IAccountContextService accountContextService,
            IAccountService accountService, IFollowService followService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _accountContextService = accountContextService;
            _accountService = accountService;
            _followService = followService;
        }

        public async Task<bool> FollowAsync(int donorId, int charityId)
        {
            // check the donor
            var donor = await _unitOfWork.Donor.GetAsync(donorId);

            if (donor == null)
                return false;

            // check the charity
            var charity = await _unitOfWork.Charity.GetAsync(charityId);
            if (charity == null)
                return false;

            var successed = await _followService.FollowAsync(donorId, charityId);

            if (!successed)
                return false;

            // Send Notification
            await _notificationService.SendNotificationAsync(donorId, [charityId], NotificationsTypeEnum.NewFollower);

            return true;
        }

        public async Task<bool> UnFollowAsync(int donorId, int charityId)
        {
            // check the donor
            var donor = await _unitOfWork.Donor.GetAsync(donorId);
            if (donor == null)
                return false;

            // check the charity
            var charity = await _unitOfWork.Charity.GetAsync(charityId);
            if (charity == null)
                return false;

            return await _followService.UnFollowAsync(donorId, charityId);

        }
        public async Task<bool> IsFollowedAsync(int donorId, int charityId)
        {
            // check the donor
            var donor = await _unitOfWork.Donor.GetAsync(donorId);
            if (donor == null)
                return false;

            // check the charity
            var charity = await _unitOfWork.Charity.GetAsync(charityId);
            if (charity == null)
                return false;

            return await _followService.IsFollowedAsync(donorId, charityId);
        }

        public async Task<List<int>> GetFollowsAsync(int donorId)
        {
            // check the donor
            var donor = await _unitOfWork.Donor.GetAsync(donorId);
            if (donor == null)
                throw new Exception("Donor is not valied");

            return await _unitOfWork.Donor.GetFollowsAsync(donorId);
        }

        public async Task<List<int>> GetSubscriptionsAsync(int donorId)
        {
            // check the donor
            var donor = await _unitOfWork.Donor.GetAsync(donorId);
            if (donor == null)
                throw new Exception("Donor is not valied");

            return await _unitOfWork.Donor.GetSubscriptionsAsync(donorId);
        }

        public async Task<bool> DonateToCharityAsync(DonationDto model)
        {
            // Check the donor
            var donor = await _unitOfWork.Donor.GetAsync(model.DonorId);
            if (donor == null)
                throw new Exception($"Donor with id {model.DonorId} not found");

            // Check the charity
            var charity = await _unitOfWork.Charity.GetAsync(model.CharityOrCampaignId);
            if (charity == null)
                throw new Exception($"Charity with id {model.CharityOrCampaignId} not found");

            // Check the state of the Status of the charity
            if (charity.Status != CharityStatusEnum.Approved)
                throw new Exception($"Charity is not approved yet.");

            var donation = new Donation
            {
                DonorId = model.DonorId,
                TotalAmount = model.TotalAmount,
                NetAmountToCharity = model.TotalAmount - (0.02m * model.TotalAmount),
            };
            await _unitOfWork.SaveAsync();

            var charityDonation = new CharityDonation
            {
                DonationId = donation.Id,
                charityID = model.CharityOrCampaignId
            };
            await _unitOfWork.SaveAsync();


            return true;
        }

        public Task<bool> DonateToCampaignAsync(DonationDto model)
        {
            throw new NotImplementedException();
        }
    }
}
