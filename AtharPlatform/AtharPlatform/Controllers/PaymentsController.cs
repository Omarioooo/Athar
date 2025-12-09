using AtharPlatform.DTO;
using AtharPlatform.DTOs;
using AtharPlatform.Repositories;
using AtharPlatform.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymob;
    private readonly IUnitOfWork _unit;

    public PaymentsController(IPaymentService paymob, IUnitOfWork unit)
    {
        _paymob = paymob;
        _unit = unit;
    }

    [HttpPost("create")]
    //[Authorize(Roles = "Donor")]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto model)
    {
        if (model == null)
            return BadRequest("Invalid model");



        var result = await _paymob.CreatePaymentAsync(model);



        return Ok(new
        {
            result.DonationId,
            result.PaymentUrl,
            result.PaymentId
        });
    }


    [HttpPost("callback")]
    public async Task<IActionResult> PaymentCallback([FromForm] PaymobCallbackDto dto)
    {
        #region Old Version 
        //var donation = await _unit.Donations
        //    .FirstOrDefaultAsync(d => d.PaymentID == dto.paymentId);

        //if (donation == null)
        //    return NotFound();

        //if (dto.success)
        //{
        //    donation.DonationStatus = TransactionStatusEnum.SUCCESSED;
        //    donation.TransactionId = dto.transactionId;
        //    donation.NetAmountToCharity = dto.amount * (1 - donation.PlatformFee);

        //    // تحديث مبلغ الحملة
        //    var campaignDonation = await _unit.CampaignDonations
        //        .FirstOrDefaultAsync(c => c.DonationId == donation.Id);

        //    if (campaignDonation != null)
        //    {
        //        var campaign = await _unit.Campaigns.GetAsync(campaignDonation.CampaignId);
        //        if (campaign != null)
        //            campaign.RaisedAmount += (double)donation.NetAmountToCharity;
        //    }
        //}
        //else
        //{
        //    donation.DonationStatus = TransactionStatusEnum.FAILED;
        //}

        //await _unit.SaveAsync();

        //return Ok($"received\nTotalAmount: {donation.TotalAmount}\nNetAmountToCharity: {donation.NetAmountToCharity}"); 
        #endregion
        var data = new Dictionary<string, string>
    {

        { "success", dto.success.ToString() },
        { "id", dto.transactionId },
        { "amount", dto.amount.ToString() }
    };

        await _paymob.HandleWebHookAsync(data);

        return Ok("Callback handled successfully");
    }

    [HttpGet("{donationId}/status")]
    public async Task<IActionResult> GetDonationStatus(int donationId)
    {
        var donation = await _unit.Donations.FirstOrDefaultAsync(d => d.Id == donationId);
        if (donation == null)
            return NotFound(new { message = "Donation not found" });

        return Ok(new
        {

            donation.Id,
            donation.DonationStatus,
            donation.TotalAmount,
            donation.NetAmountToCharity
        });
    }


    [HttpGet("charity/{charityId}/total-donations")]
    public async Task<IActionResult> GetTotalDonationsForCharity(int charityId)
    {
        if (charityId <= 0)
            return BadRequest("Invalid charity ID");

        // Check if charity exists
        var charityExists = await _unit.Charities.GetByIdAsync(charityId);
        if (charityExists == null)
            return NotFound("Charity not found");

        var total = await _paymob.GetTotalDonationsForCharityAsync(charityId);

        return Ok(new
        {
            CharityId = charityId,
            CharityName = charityExists.Name,
            TotalDonation = total
        });
    }
}
