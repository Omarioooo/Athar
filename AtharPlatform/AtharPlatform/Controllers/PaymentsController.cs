using AtharPlatform.DTO;
using AtharPlatform.DTOs;
using AtharPlatform.Models;
using AtharPlatform.Models.Enums;
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

    // 1️⃣ Create Payment
    [HttpPost("create")]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto model)
    {
        if (model == null)
            return BadRequest("Invalid model");

      
        var donation = new Donation
        {
            DonorId = model.DonorId,
            TotalAmount = (decimal)model.Amount,
            DonationStatus = TransactionStatusEnum.PENDING,
            Currency = "EGP",
            CharityId = model.CharityId,
            CreatedAt = DateTime.UtcNow
        };

        await _unit.Donations.AddAsync(donation);
        await _unit.SaveAsync();

       
        if (model.CampaignId != null)
        {
            var c = new CampaignDonation
            {
                DonationId = donation.Id,
                CampaignId = model.CampaignId
            };
            await _unit.CampaignDonations.AddAsync(c);
        }

       
        if (model.CharityId != null)
        {
            var c = new CharityDonation
            {
                DonationId = donation.Id,
                charityID = model.CharityId
            };
            await _unit.CharityDonations.AddAsync(c);
        }

        await _unit.SaveAsync();

        
        var result = await _paymob.CreatePaymentAsync(model);

        donation.PaymentID = result.PaymentId;
        donation.MerchantOrderId = result.MerchantOrderId;
        await _unit.SaveAsync();

        return Ok(new
        {
            donationId = donation.Id,
            result.PaymentUrl,
            result.PaymentId
        });
    }

    
    [HttpPost("callback")]
    public async Task<IActionResult> PaymentCallback([FromForm] PaymobCallbackDto dto)
    {
        var donation = await _unit.Donations
            .FirstOrDefaultAsync(d => d.PaymentID == dto.paymentId);

        if (donation == null)
            return NotFound();

        if (dto.success)
        {
            donation.DonationStatus = TransactionStatusEnum.SUCCESSED;
            donation.TransactionId = dto.transactionId;
            donation.NetAmountToCharity = dto.amount * (1 - donation.PlatformFee);

            // تحديث مبلغ الحملة
            var campaignDonation = await _unit.CampaignDonations
                .FirstOrDefaultAsync(c => c.DonationId == donation.Id);

            if (campaignDonation != null)
            {
                var campaign = await _unit.Campaigns.GetAsync(campaignDonation.CampaignId);
                if (campaign != null)
                    campaign.RaisedAmount += (double)donation.NetAmountToCharity;
            }
        }
        else
        {
            donation.DonationStatus = TransactionStatusEnum.FAILED;
        }

        await _unit.SaveAsync();

        return Ok("received");
    }

    
    [HttpGet("{donationId}/status")]
    public async Task<IActionResult> GetDonationStatus(int donationId)
    {
        var donation = await _unit.Donations.FirstOrDefaultAsync(d => d.Id == donationId);
        if (donation == null) return NotFound();

        return Ok(new
        {
            donation.DonationStatus,
            donation.TotalAmount,
            donation.NetAmountToCharity
        });
    }
}
