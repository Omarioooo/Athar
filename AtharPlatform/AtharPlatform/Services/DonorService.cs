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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DonorService(IUnitOfWork unitOfWork, ISubscriptionService subscriptionService,
            INotificationService notificationService, IAccountContextService accountContextService,
            IAccountService accountService, IFollowService followService,IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _accountContextService = accountContextService;
            _accountService = accountService;
            _followService = followService;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<DonorProfileDto> GetDonorByIdAsync(int id)
        {
            var donor = await _unitOfWork.Donors.GetDonorFullProfileAsync(id);
            if (donor == null)
                throw new Exception("Donor not found");

            var baseUrl = _httpContextAccessor.HttpContext != null
                ? $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}"
                : "";

            var donations = donor.Donations?.Where(c => c?.Donation != null).Select(c => c.Donation!).ToList();
            var followsCount = donor.Follows?.Count ?? 0;

            return new DonorProfileDto
            {
                FirstName = donor.FirstName,
                LastName = donor.LastName,
                Email = donor.Account.Email,
                ImageUrl = donor.Account?.ProfileImage != null ? $"{baseUrl}/api/account/users/profile-image/{donor.Id}" : null,
                Country = donor.Account?.Country,
                City = donor.Account?.City,
                DonationsCount = donations?.Count ?? 0,
                FollowingCount = followsCount,
                TotalDonationsAmount = donations?.Sum(x => (double)x.NetAmountToCharity) ?? 0,
                DonationsHistory = donations?.Select(d => new DonationsHistoryDto
                {
                    DonationId = d.Id,
                    Amount = (double)d.NetAmountToCharity,
                    DonationDate = d.CreatedAt,
                    Currency = d.Currency,
                    Status = d.DonationStatus.ToString(),
                    CampaignId = d.CampaignId,
                    CharityId = d.CharityId
                }).ToList() ?? new List<DonationsHistoryDto>()
            };
        }

        public async Task<Donor> GetDonorFullProfileAsync(int id)
        {
            return await _unitOfWork.Donors.GetDonorFullProfileAsync(id);
        }

    }
}
