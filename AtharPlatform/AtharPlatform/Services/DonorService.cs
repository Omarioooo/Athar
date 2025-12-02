using Ardalis.GuardClauses;
using AtharPlatform.DTO;
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

        // يوثف وائل
        public async Task<DonorProfileDto> GetDonorById(int id)
        {
            var donor = _unitOfWork.Donors.GetByIdAsync(id);
            if (donor == null)
                throw new NotFoundException($"{id}", $"Donor with id {id} not found");



            DonorProfileDto dto = new DonorProfileDto();

            return dto;
        }


        public async Task<bool> DonateToCharityAsync(DonationDto model)
        {
            // Check the donor
            var donor = await _unitOfWork.Donors.GetAsync(model.DonorId);
            if (donor == null)
                throw new Exception($"Donor with id {model.DonorId} not found");

            // Check the charity
            var charity = await _unitOfWork.Charities.GetAsync(model.CharityOrCampaignId);
            if (charity == null)
                throw new Exception($"Charity with id {model.CharityOrCampaignId} not found");

            // Check the state of the Status of the charity
            if (charity.Status != CharityStatusEnum.Approved)
                throw new Exception($"Charity is not approved yet.");

            var donation = new Donation
            {
                DonorId = model.DonorId,
                TotalAmount = (decimal)model.TotalAmount,
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
