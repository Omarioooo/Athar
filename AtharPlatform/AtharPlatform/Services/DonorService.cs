using AtharPlatform.DTO;
using AtharPlatform.DTOs;
using AtharPlatform.Models;
using AtharPlatform.Models.Enum;
using AtharPlatform.Models.Enums;
using AtharPlatform.Repositories;
using System.Buffers.Text;
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

        public async Task<bool> UpdateDonorAsync(int donorId, DonorUpdateDto dto)
        {
            var donor = await _unitOfWork.Donors.GetAsync(donorId);
            if (donor == null)
                throw new Exception($"Donor with id {donorId} not found");

            // Update basic data
            donor.FirstName = dto.FirstName ?? donor.FirstName;
            donor.LastName = dto.LastName ?? donor.LastName;

            donor.Account.City = dto.City ?? donor.Account.City;
            donor.Account.Country = dto.Country ?? donor.Account.Country;

            // Update Image if exists
            if (dto.Image != null)
            {
                using var ms = new MemoryStream();
                await dto.Image.CopyToAsync(ms);
                donor.Account.ProfileImage = ms.ToArray();
            }

            await _unitOfWork.Donors.UpdateAsync(donor);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<bool> DeleteDonorAsync(int id)
        {
            // 1 - جلب الدونور مع كل علاقاته
            var donor = await _unitOfWork.Donors.getDonorWithId(id);
            if (donor == null)
                throw new Exception("Donor not found.");

             
            await _unitOfWork.Donors.DeleteDonorAsync(id);
            
            await _unitOfWork.SaveAsync();

            return true;
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
                //ImageUrl = donor.Account?.ProfileImage != null ? $"{baseUrl}/api/account/users/profile-image/{donor.Id}" : null,
               ImageUrl = donor.Account?.ProfileImage != null ? $"{baseUrl}/api/Donor/profile-image/{donor.Id}" : null,

                Country = donor.Account?.Country,
                City = donor.Account?.City,
                DonationsCount = donations?.Count ?? 0,
                FollowingCount = followsCount,
                TotalDonationsAmount = donations?.Sum(x => (double)x.NetAmountToCharity) ?? 0,
                DonationsHistory = donations?.Select(d => new DonationsHistoryDto
                {
                    DonationId = d.Id,
                    Amount = (double)d.NetAmountToCharity,
                    Date = d.CreatedAt,
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

        public async Task<DonorAtharDto> GetAtharByDonorIdAsync(int id)
        {
            // Check the donor
            var donor = await _unitOfWork.Donors.getDonorWithId(id);
            if (donor == null)
                throw new Exception($"Donor with id {id} not found");

            //// 1) جيب كل الفولوز
            var Follows = await _unitOfWork.Follows.GetAllAsync();
            var resultFollows = new List<FollowAtharHistoryDto>();

            foreach (var f in Follows.Where(a => a.DonorId == id))
            {
                var charity = await _unitOfWork.Charities.GetAsync(f.CharityId);

                resultFollows.Add(new FollowAtharHistoryDto
                {
                    charityId = f.CharityId,
                    charityName = charity?.Name,
                    charityImageUrl = GetImgUrl(f.CharityId, charity?.ImageUrl, charity?.Account?.ProfileImage)
                });
            }

            // charity donations
            var donations = donor.Donations.ToList();
            var donationsAthar = new List<DonorDonationAtharHistoryDto>();

            decimal total = 0;
            foreach (var don in donations)
            {
                var campaign = await _unitOfWork.Campaigns.GetAsync(don.CampaignId);
                var charity = await _unitOfWork.Charities.GetCharityByCampaignAsync(don.CampaignId);
                var donation = await _unitOfWork.Donations.FindAsync(don.DonorId);

                donationsAthar.Add(new DonorDonationAtharHistoryDto
                {
                    CampaignName = campaign.Title,
                    CharityName = charity.Name,
                    DonationId = don.DonationId,
                    Amount = donation.TotalAmount,
                    Date = donation.CreatedAt
                });

                total += donation.TotalAmount;
            }

            return new DonorAtharDto()
            {
                DonationsAmount = total,
                donations = donationsAthar,
                follows = resultFollows
            };
        }
        private string? GetImgUrl(int id, string charityImageURL, byte[] charityAccountImage)
        {
            var http = _httpContextAccessor.HttpContext;
            var baseUrl = http != null
                ? $"{http.Request.Scheme}://{http.Request.Host}"
                : "";

            string? imageUrl;

            if (!string.IsNullOrEmpty(charityImageURL))
            {
                imageUrl = charityImageURL;
            }
            else if (charityAccountImage != null)
            {
                imageUrl = $"{baseUrl}/api/charities/profile-image/{id}";
            }
            else
            {
                imageUrl = null;
            }

            return imageUrl;
        }

 

    }
}
