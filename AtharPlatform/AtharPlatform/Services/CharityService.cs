using System;
using AtharPlatform.Dtos;
using AtharPlatform.DTOs;
using AtharPlatform.Models;
using AtharPlatform.Models.Enums;
using AtharPlatform.Repositories;

namespace AtharPlatform.Services
{
    public class CharityService : ICharityService
    {
        private readonly Context _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileService _fileService;

        public CharityService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, Context context, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _fileService = fileService;
        }


        public async Task<CharityStatusDto> GetCharityStatisticsAsync(int charityId)
        {
             var charity = await _context.Charities
                .FirstOrDefaultAsync(c => c.Id == charityId);

            if (charity == null)
                throw new Exception("Charity not found");

             int followsCount = await _context.Follows
                .Where(f => f.CharityId == charityId)
                .CountAsync();

             
            int campaignsCount = await _context.Campaigns
                .Where(c => c.CharityID == charityId)
                .CountAsync();

            
            decimal totalDonations = await _context.Donations
                .Where(d => d.CharityId == charityId && d.DonationStatus == TransactionStatusEnum.SUCCESSED)
                .SumAsync(d => d.NetAmountToCharity);

             
            int donationsCount = await _context.Donations
                .Where(d => d.CharityId == charityId)
                .CountAsync();

             
            var campaignsList = await _context.Campaigns
                .Where(c => c.CharityID == charityId)
                .ToListAsync();

             
            int totalContent = 0;
            foreach (var c in campaignsList)
            {
                var contentsCount = await _context.Contents
                    .Where(ct => !ct.IsDeleted && ct.CampaignId == c.Id)
                    .CountAsync();

                totalContent += contentsCount;
            }

 
            return new CharityStatusDto
            {
                FollowsCount = followsCount,
                CampaignsCount = campaignsCount,
                TotalIncome = totalDonations,
                DonationsCount = donationsCount,
                ContentCount = totalContent
            };
        }


        public async Task<List<CharityApplicationResponseDto>> GetAllApplicationsForCharityAsync(int charityId)
        {
            // 1) Get volunteer  to charity
            var volunteerSlots = await _unitOfWork.CharityVolunteers.GetByCharityIdAsync(charityId);

            var volunteerApps = volunteerSlots
                .SelectMany(v => v.VolunteerApplications)
                .Select(v => new CharityApplicationResponseDto
                {
                    Id = v.Id,
                    Type = "Volunteer",
                    Name = v.FirstName + " " + v.LastName,
                    Phone = v.PhoneNumber,
                    // Country = v.Country,
                    //City = v.City,
                    ///Age = v.Age,
                    Description = $"ارغب في التطوع لجمعية {v.CharityVolunteer?.Charity?.Name}",
                    Date = v.CharityVolunteer.Date,
                    //IsFirstTime = v.IsFirstTime
                })
                .ToList();

            // 2) Get vendor  to charity
            var vendorSlots = await _unitOfWork.CharityVendorOffers.GetByCharityIdAsync(charityId);
            vendorSlots ??= new List<CharityVendorOffer>();

            var vendorOffers = vendorSlots
                .Where(v => v?.VendorOffers != null)
                .SelectMany(v => v.VendorOffers!)
                .Select(v => new CharityApplicationResponseDto
                {
                    Id = v.Id,
                    Type = "VendorOffer",
                    Name = v.VendorName,
                    Phone = v.PhoneNumber,
                    // Country = v.Country,
                    // City = v.City,
                    // ItemName = v.ItemName,
                    // Quantity = v.Quantity,
                    Description = v.Description,
                    // PriceBefore = v.PriceBeforDiscount,
                    /// PriceAfter = v.PriceAfterDiscount,
                    Date = v.CharityVendorOffer.Date
                })
                .ToList();



            return volunteerApps
                .Concat(vendorOffers)
                .OrderBy(c => c.Date)
                .ToList();
        }

        public async Task<VendorOfferDTO> GetVendorOfferForCharityByIdAsync(int offerId)
        {
            var offer = await _unitOfWork.VendorOffers.GetAsync(offerId);

            if (offer == null)
                throw new Exception("offer not found");

            var charityoffer = await _unitOfWork.CharityVendorOffers.GetByIdAsync(offerId);

            return new VendorOfferDTO
            {
                Id = offer.Id,
                VendorName = offer.VendorName,
                PhoneNumber = offer.PhoneNumber,
                City = offer.City,
                Country = offer.Country,
                Description = offer.Description,
                ItemName = offer.ItemName,
                Quantity = offer.Quantity,
                PriceAfterDiscount = offer.PriceAfterDiscount,
                PriceBeforDiscount = offer.PriceBeforDiscount,
                Date = charityoffer?.Date ?? DateTime.Now
            };
        }

        public async Task<VolunteerApplicationDTO> GetVolunteerOfferForCharityByIdAsync(int offerId)
        {
            var application = await _unitOfWork.VolunteerApplications.GetAsync(offerId);

            if (application == null)
                throw new Exception("application not found");

            var charityOffer = await _unitOfWork.CharityVolunteers.GetByIdAsync(application.Id);

            return new VolunteerApplicationDTO
            {
                Id = application.Id,
                FirstName = application.FirstName,
                LastName = application.LastName,
                Age = application.Age,
                City = application.City,
                Country = application.Country,
                PhoneNumber = application.PhoneNumber,
                Description = $"ارغب في التطوع لجمعية {charityOffer?.Charity?.Name}" ?? "أرغب في التطوع",
                Date = charityOffer?.Date ?? DateTime.Now
            };
        }

        public async Task<CharityProfileDto> GetCharityByIdAsync(int id)
        {
            var http = _httpContextAccessor.HttpContext;
            var baseUrl = http != null
                ? $"{http.Request.Scheme}://{http.Request.Host}"
                : "";

            var charity = await _unitOfWork.Charities.GetCharityFullProfileAsync(id);

            if (charity == null || charity.IsActive == false)//علشان ميرجعش المحذوف
                return null;


            string? imageUrl;

            if (!string.IsNullOrEmpty(charity.ImageUrl))
            {
                imageUrl = charity.ImageUrl;
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

        public async Task<CharityViewDto?> GetCharityViewAsync(int id)
        {
            try
            {
                var charity = await _context.Charities
                    .Include(c => c.Campaigns)
                    .Include(c => c.Follows)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (charity == null || charity.IsActive == false)
                    return null;

                var campaignIds = charity.Campaigns.Select(c => c.Id).ToList();

                var contents = await _context.Contents
                    .Include(c => c.Reactions)
                    .Where(c => campaignIds.Contains(c.CampaignId))
                    .ToListAsync();

                var dto = new CharityViewDto
                {
                    Name = charity.Name,
                    Description = charity.Description,
                    ImgUrl = charity.ImageUrl,
                    NumOfFollowers = charity.Follows.Count,
                    NumOfCampaigns = charity.Campaigns.Count,

                    Campaigns = charity.Campaigns.Select(c => new CharityViewCampaignDto
                    {
                        Id = c.Id,
                        Title = c.Title,
                        ImgUrl = c.ImageUrl,
                        Description = c.Description,
                        RaisedAmount = c.RaisedAmount,
                        GoaldAmount = c.GoalAmount,
                        StartDate = DateOnly.FromDateTime(c.StartDate),
                        EndDate = DateOnly.FromDateTime(c.StartDate.AddDays(c.Duration))
                    }).ToList(),

                    Contents = contents.Select(x => new CharityViewContentDto
                    {
                        Id = x.Id,
                        Description = x.Description,
                        ImgUrl = x.PostImage != null
                            ? $"data:image/png;base64,{Convert.ToBase64String(x.PostImage)}"
                            : null,
                        TotalReactions = x.Reactions.Count
                    }).ToList()
                };

                return dto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateCharityDto model)
        {
            try
            {
                // 1 - جلب الجمعية
                var charity = await _unitOfWork.Charities.GetAsync(id);
                if (charity == null)
                    throw new ArgumentNullException(nameof(model), "Charity not found");

                // 2 - تحديث البيانات الأساسية
                charity.Name = model.CharityName;
                charity.Description = model.Description;

                if (charity.Account == null)
                    throw new Exception("Charity account not found.");

                charity.Account.City = model.City;
                charity.Account.Country = model.Country;

                // 3 - تحديث الصورة لو تم رفع صورة جديدة
                if (model.ProfileImage != null && model.ProfileImage.Length > 0)
                {
                    // حفظ الصورة كملف على السيرفر (مثل CharityRegisterAsync)
                    var imageUrl = await _fileService.SaveFileAsync(model.ProfileImage, "charities");
                    charity.ImageUrl = imageUrl;
                }

                // 4 - حفظ التغييرات
                await _unitOfWork.Charities.UpdateAsync(charity);
                await _unitOfWork.SaveAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<CharityJoinDto>> GetCharityJoinApplicationsAsync()
        {
            var http = _httpContextAccessor.HttpContext;
            var baseUrl = http != null
                ? $"{http.Request.Scheme}://{http.Request.Host}"
                : "";

            var charities = await _unitOfWork.Charities.GetAllAsync();
            if (charities == null)
                throw new ArgumentNullException("Charities not found");

            List<CharityJoinDto> apps = new List<CharityJoinDto>();

            foreach (var charity in charities)
            {
                if (charity.Status != Models.Enums.CharityStatusEnum.Pending)
                    continue;

                string? imageUrl;

                if (!string.IsNullOrEmpty(charity.ImageUrl))
                {
                    imageUrl = charity.ImageUrl;
                }
                else if (charity.Account?.ProfileImage != null)
                {
                    imageUrl = $"{baseUrl}/api/charities/profile-image/{charity.Id}";
                }
                else
                {
                    imageUrl = null;
                }

                apps.Add(new CharityJoinDto
                {
                    Id = charity.Id,
                    Name = charity.Name,
                    City = charity.Account.City,
                    Country = charity.Account.Country,
                    CreatedAt = charity.Account.CreatedAt,
                    Description = charity.Description,
                    email = charity.Account.Email,
                    ImageUrl = imageUrl,
                    VerificationDocument = charity.VerificationDocument != null
                ? $"data:image/png;base64,{Convert.ToBase64String(charity.VerificationDocument)}"
                : null,
                });

            }

            return apps;

        }


    }

}
