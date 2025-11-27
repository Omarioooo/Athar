using AtharPlatform.DTOs;
using AtharPlatform.Models;
using AtharPlatform.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AtharPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly IContentService _contentService;

        public ContentController(IContentService contentService)
        {
            _contentService = contentService;
        }



        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 12)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Page number and page size must be greater than zero.");

            var contents = await _contentService.GetPagedAllAsync(page,pageSize);

            if (contents == null || !contents.Items.Any())
            {
                return NotFound("No contents found");
            }
            return Ok(contents);
                
        }


        [HttpGet("followed/{donorId}/paged")]
        [Authorize(Roles = "CharityAdmin,Donor")]
        public async Task<IActionResult> GetFollowedContent(int donorId, int page = 1, int pageSize = 12)
        {
            if (donorId <= 0)
                return BadRequest("Invalid donor ID");

            var result = await _contentService.GetFollowedCharitiesContentAsync(donorId, page, pageSize);

            if (result == null || result.Total == 0 || result.Items == null || !result.Items.Any())
                return NoContent();

            return Ok(result);
        }




        [HttpPost("create")]
        [Consumes("multipart/form-data")]// بيقول علشان Swagger يعرف انه بيتم رفع صورة
        [Authorize(Roles = "CharityAdmin")]
        public async Task<IActionResult> CreateContent([FromForm] CreateContentDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var content = await _contentService.CreateContentAsync(dto);
            return Ok(content);
        }


        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid content ID");

            var content = await _contentService.GetByIdAsync(id);
            if (content == null || content.PostImage == null)
                return NotFound("No image found");

            return File(content.PostImage, "image/png");
        }



        //[HttpGet("campaign/{campaignId}")]
        //public async Task<IActionResult> GetByCampaignId(int campaignId)
        //{
        //    if (campaignId <= 0)
        //        return BadRequest("Invalid campaign ID");

        //    var contents = await _contentService.GetByCampaignIdAsync(campaignId);
        //    if (contents == null || !contents.Any())
        //        return NotFound("No contents found for this campaign");

        //    return Ok(contents);
        //}


        [HttpGet("campaign/{campaignId}/paged")]
        public async Task<IActionResult> GetPagedByCampaign(int campaignId, int page = 1, int pageSize = 12)
        {
            if (campaignId <= 0)
                return BadRequest("Invalid campaign ID");

            var result = await _contentService.GetPagedByCampaignIdAsync(campaignId, page, pageSize);
            if (result == null || !result.Items.Any())
                return NotFound("No contents found for this campaign");

            return Ok(result);
        }


        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "CharityAdmin")]
        public async Task<IActionResult> UpdateContent(int id, [FromForm] UpdateContentDTO dto)
        {
            try
            {
                var updatedContent = await _contentService.UpdateContentAsync(id, dto);
                return Ok(updatedContent);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }



        [HttpDelete("{id}")]
       [Authorize(Roles = "CharityAdmin")]
        public async Task<IActionResult> DeleteContent(int id)
        {
            var result = await _contentService.DeleteContentAsync(id);
            if (!result)
                return NotFound("Content not found");

            return NoContent(); 
        }



        [HttpGet("charity/{charityId}/paged")]
        public async Task<IActionResult> GetByCharityId(int charityId, int page = 1, int pageSize = 12)
        {
            if (charityId <= 0)
                return BadRequest("Invalid Charity ID");

            if (page <= 0 || pageSize <= 0)
                return BadRequest("Page number and page size must be greater than zero");

            var pagedContents = await _contentService.GetPagedByCharityIdAsync(charityId, page, pageSize);

            if (pagedContents == null|| !pagedContents.Items.Any())
                return NotFound("No contents found for this Charity");

            return Ok(pagedContents);
        }



        //علشان تدور باسم الجمعية او اسم الحملة عنوانها يعني
        [HttpGet("search")]
        [Authorize(Roles = "CharityAdmin,Donor,SuperAdmin")]
        public async Task<IActionResult> SearchContents([FromQuery] string Word)
        {
            if (string.IsNullOrWhiteSpace(Word))
                return BadRequest("Keyword cannot be empty.");

            var results = await _contentService.SearchContentsAsync(Word);
            return Ok(results);
        }


    }


}
