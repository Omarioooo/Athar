using AtharPlatform.Dtos;
using AtharPlatform.DTOs;
using AtharPlatform.Repositories;

namespace AtharPlatform.Services
{
    public class CharityService : ICharityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharityService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CharityProfileDto> GetCharityByIdAsync(int id)
        {
            var http = _httpContextAccessor.HttpContext;
            var baseUrl = http != null
                ? $"{http.Request.Scheme}://{http.Request.Host}"
                : "";

            var charity = await _unitOfWork.Charities.GetCharityFullProfileAsync(id);

            if (charity == null)
                return null;

       
            string? imageUrl;

            if (!string.IsNullOrEmpty(charity.ImageUrl))
            {
                imageUrl = charity.ImageUrl; // خارجي → رجعه كما هو
            }
            else if (charity.Account?.ProfileImage != null)
            {
                imageUrl = $"{baseUrl}/api/charities/profile-image/{charity.Id}";
            }
            else
            {
                imageUrl = null;
            }


            // Total raised across all campaigns
            double totalRaised =
                charity.Campaigns?
                    .SelectMany(c => c.CampaignDonations ?? new List<CampaignDonation>())
                    .Where(cd => cd.Donation != null)
                    .Sum(cd => (double)cd.Donation.NetAmountToCharity)
                ?? 0;

            // Mini campaigns list
            var campaigns =
                charity.Campaigns?
                .Select(c => new MiniCampaignDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    GoalAmount = (double)c.GoalAmount,
                    RaisedAmount = c.CampaignDonations?
                        .Where(cd => cd.Donation != null)
                        .Sum(cd => (double)cd.Donation.NetAmountToCharity)
                        ?? 0
                })
                .ToList()
                ?? new List<MiniCampaignDto>();



            return new CharityProfileDto
            {
                Name = charity.Name,
                ImageUrl = imageUrl,
                Description = charity.Description,
                Address = $"{charity.Account?.Country}, {charity.Account?.City}",

                CampaignsCount = campaigns.Count,
                TotalRaised = totalRaised,
                FollowersCount = charity.Follows?.Count ?? 0,
                status = charity.Status,
                Campaigns = campaigns
            };
        }

        public async Task<Charity> GetCharityFullProfileAsync(int id)
        {
            return await _unitOfWork.Charities.GetCharityFullProfileAsync(id);
        }
    }
}
