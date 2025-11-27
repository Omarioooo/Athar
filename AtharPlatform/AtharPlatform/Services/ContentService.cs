using AtharPlatform.Dtos;
using AtharPlatform.DTOs;
using AtharPlatform.Repositories;


namespace AtharPlatform.Services
{
    public class ContentService : IContentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IContentRepository _contentRepository;
        public ContentService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IContentRepository contentRepository)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _contentRepository = contentRepository;
        }

        public async Task<PaginatedResultDto<ContentList_Detailes_DTO>> GetPagedAllAsync(int page, int pageSize)
        {
            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

            var query = _unitOfWork.Contents.GetAll()
                .Include(c => c.Campaign)
                .ThenInclude(ca => ca.Charity);

         
            var totalItems = await query.CountAsync();

            var items = await query
                .OrderBy(c => Guid.NewGuid()) //بشكل عشوائي يعني الترتيب

                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new ContentList_Detailes_DTO
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    CreatedAt = c.CreatedAt,
                    ImageUrl = $"{baseUrl}/api/content/image/{c.Id}",

                    CampaignId = c.CampaignId,
                    CampaignTitle = c.Campaign.Title,
                    CharityId = c.Campaign.CharityID,
                    CharityName = c.Campaign.Charity.Name
                })
                .ToListAsync();

            return new PaginatedResultDto<ContentList_Detailes_DTO>
            {
                Items = items,
                Total = totalItems,
                Page = page,
                PageSize = pageSize
            };
        }



        //الجمعيات الي عملها فولو بس
        public async Task<PaginatedResultDto<ContentList_Detailes_DTO>>GetFollowedCharitiesContentAsync(int donorId, int page, int pageSize)
        {
            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

            
            var followedCharities = await _unitOfWork.Follows.GetAll()
                .Where(f => f.DonorId == donorId)
                .Select(f => f.CharityId)
                .ToListAsync();

            if (!followedCharities.Any())
                return new PaginatedResultDto<ContentList_Detailes_DTO>
                {
                    Items = new List<ContentList_Detailes_DTO>(),
                    Page = page,
                    PageSize = pageSize,
                    Total = 0
                };

           
            var query = _unitOfWork.Contents.GetAll()
                .Include(c => c.Campaign)
                .ThenInclude(ca => ca.Charity)
                .Where(c => followedCharities.Contains(c.Campaign.CharityID));

            var totalItems = await query.CountAsync();

            var items = await query
                // .OrderByDescending(c => c.CreatedAt) // لو نعمللها بالترتيب من الاحدث يعني
                .OrderBy(c => Guid.NewGuid())//ننعرض بشكل عشوائي
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new ContentList_Detailes_DTO
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    CreatedAt = c.CreatedAt,
                    ImageUrl = $"{baseUrl}/api/content/image/{c.Id}",

                    CampaignId = c.CampaignId,
                    CampaignTitle = c.Campaign.Title,
                    CharityId = c.Campaign.CharityID,
                    CharityName = c.Campaign.Charity.Name
                })
                .ToListAsync();

            return new PaginatedResultDto<ContentList_Detailes_DTO>
            {
                Items = items,
                Total = totalItems,
                Page = page,
                PageSize = pageSize
            };
        }












        public async Task<Content> CreateContentAsync(CreateContentDTO dto)
        {
            var content = new Content
            {
                Title = dto.Title,
                Description = dto.Description,
                CampaignId = dto.CampaignId,
                CreatedAt = DateTime.UtcNow
            };
            // لو رفع صورة → نحولها لـ byte[]
            if (dto.PostImage != null)
            {
                using var ms = new MemoryStream();
                await dto.PostImage.CopyToAsync(ms);
                content.PostImage = ms.ToArray();
            }

            await _unitOfWork.Contents.AddAsync(content);
            await _unitOfWork.SaveAsync();

            return content;
        }


        public async Task<Content?> GetByIdAsync(int id)
        {
            return await _unitOfWork.Contents.GetByIdAsync(id);
        }



        public async Task<List<Content>> GetPostsByCampaignAsync(int campaignId)
        {
            return await _unitOfWork.Contents.GetPostsByCampaignAsync(campaignId);
        }

        public async Task<List<ContentListDTO>> GetByCampaignIdAsync(int campaignId)
        {
            var contents = await _unitOfWork.Contents.GetByCampaignIdAsync(campaignId);

            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://" +
                          $"{_httpContextAccessor.HttpContext.Request.Host}";

            return contents.Select(c => new ContentListDTO
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                CreatedAt = c.CreatedAt,
                ImageUrl = $"{baseUrl}/api/content/image/{c.Id}"
            }).ToList();
        }

        public async Task<PagingResponse<ContentListDTO>> GetPagedByCampaignIdAsync(int campaignId, int pageNumber, int pageSize)
        {
            var query = _contentRepository.GetAll().Where(c => c.CampaignId == campaignId).OrderByDescending(c => c.CreatedAt);

            var totalItems = query.Count();

            var items = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

            var dtos = items.Select(c => new ContentListDTO
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                CreatedAt = c.CreatedAt,
                ImageUrl = $"{baseUrl}/api/content/image/{c.Id}"
            }).ToList();

            return new PagingResponse<ContentListDTO>
            {
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Items = dtos
            };
        }


        public async Task<Content> UpdateContentAsync(int id, UpdateContentDTO dto)
        {
            var content = await _unitOfWork.Contents.GetByIdAsync(id);
            if (content == null)
                throw new Exception("Content not found");

            if (!string.IsNullOrEmpty(dto.Title))
                content.Title = dto.Title;
            if (!string.IsNullOrEmpty(dto.Description))
                content.Description = dto.Description;

            if (dto.PostImage != null)
            {
                using var ms = new MemoryStream();
                await dto.PostImage.CopyToAsync(ms);
                content.PostImage = ms.ToArray();
            }

            await _unitOfWork.SaveAsync();
            return content;
        }



        // ContentService
        public async Task<bool> DeleteContentAsync(int id)
        {
            var content = await _unitOfWork.Contents.GetByIdAsync(id);
            if (content == null)
                return false;

            // soft delete
            content.IsDeleted = true;//هنا بيعمل Delete بس لسه موجود عندنا 

            await _unitOfWork.SaveAsync();
            return true;
        }


        public async Task<PagingResponse<ContentListDTO>> GetPagedByCharityIdAsync(int charityId, int pageNumber, int pageSize)
        {
            var query = _unitOfWork.Contents
                .GetAll()
                .Where(c => c.Campaign.CharityID == charityId && !c.IsDeleted)
                .OrderByDescending(c => c.CreatedAt);

            var totalItems = query.Count();
            var items = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

            var data = items.Select(c => new ContentListDTO
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                CreatedAt = c.CreatedAt,
                ImageUrl = $"{baseUrl}/api/content/image/{c.Id}"
            }).ToList();

            return new PagingResponse<ContentListDTO>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                Items = data
            };
        }



        public async Task<List<ContentListDTO>> SearchContentsAsync(string keyword)
        {
            var query = _unitOfWork.Contents.GetAll()
                .Where(c => !c.IsDeleted &&
                             (c.Campaign.Title.Contains(keyword) ||
                              c.Campaign.Charity.Name.Contains(keyword)))
                .OrderByDescending(c => c.CreatedAt);

            var contents = query.ToList();

            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";

            var dtoList = contents.Select(c => new ContentListDTO
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                CreatedAt = c.CreatedAt,
                ImageUrl = $"{baseUrl}/api/content/image/{c.Id}",

            }).ToList();

            return dtoList;
        }


    }
}