using AtharPlatform.DTO;
using AtharPlatform.Models.Enums;
using AtharPlatform.Repositories;

namespace AtharPlatform.Services
{
    public class DonationService : IDonationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymobService;

        public DonationService(
            IUnitOfWork unitOfWork,
            IPaymentService paymobService,
            IAccountContextService accountContextService)
        {
            _unitOfWork = unitOfWork;
            _paymobService = paymobService;
        }

        public async Task<string> DonateToCampaignAsync(DonationDto model)
        {
         
            var donor = await _unitOfWork.Donors.GetAsync(model.DonorId)
                        ?? throw new Exception($"Donor with id {model.DonorId} not found");

            
            var campaign = await _unitOfWork.Campaigns.GetAsync(model.CharityOrCampaignId)
                          ?? throw new Exception($"Charity with id {model.CharityOrCampaignId} not found");

            
            var donation = new Donation
            {
                DonorId = model.DonorId,
                TotalAmount =(decimal)model.TotalAmount,
                NetAmountToCharity = model.TotalAmount - (model.TotalAmount * 0.02m),
                Currency = "EGP",
                MerchantOrderId = Guid.NewGuid().ToString(),
                DonationStatus = TransactionStatusEnum.PENDING
            };

            await _unitOfWork.Donations.AddAsync(donation);
            await _unitOfWork.SaveAsync();

          
            var campaingDonation = new CampaignDonation
            {
                DonationId = donation.Id,
                CampaignId = model.CharityOrCampaignId
            };

            await _unitOfWork.CampaignDonations.AddAsync(campaingDonation);
            await _unitOfWork.SaveAsync();

           
            var paymentOutPut = await _paymobService.CreatePaymentAsync(new CreatePaymentDto
            {
                DonorId=model.DonorId,
                CharityId=model.CharityOrCampaignId,
                CampaignId = model.CharityOrCampaignId,
                DonorFirstName = donor.FirstName,
                DonorLastName = donor.LastName,
                DonorEmail = donor.Account.Email,
                Amount = model.TotalAmount,
                MerchantOrderId = donation.MerchantOrderId
            });

            donation.PaymentID = paymentOutPut.PaymentId;
            await _unitOfWork.SaveAsync();

            // Return payment link
            return paymentOutPut.PaymentUrl ?? "";
        }

        public async Task<string> DonateToCharityAsync(DonationDto model)
        {
            // Check the donor
            var donor = await _unitOfWork.Donors.GetAsync(model.DonorId)
                        ?? throw new Exception($"Donor with id {model.DonorId} not found");

            // Check the charity
            var charity = await _unitOfWork.Charities.GetAsync(model.CharityOrCampaignId)
                          ?? throw new Exception($"Charity with id {model.CharityOrCampaignId} not found");

            if (charity.Status != CharityStatusEnum.Approved)
                throw new Exception("Charity is not approved yet.");

            // Create donation
            var donation = new Donation
            {
                DonorId = model.DonorId,
                TotalAmount = (decimal)model.TotalAmount,
                NetAmountToCharity = model.TotalAmount - (model.TotalAmount * 0.02m),
                Currency = "EGP",
                MerchantOrderId = Guid.NewGuid().ToString(),
                DonationStatus = TransactionStatusEnum.PENDING
            };

            await _unitOfWork.Donations.AddAsync(donation);
            await _unitOfWork.SaveAsync();

            // Link donation to charity
            var charityDonation = new CharityDonation
            {
                DonationId = donation.Id,
                charityID = model.CharityOrCampaignId
            };

            await _unitOfWork.CharityDonations.AddAsync(charityDonation);
            await _unitOfWork.SaveAsync();

            // Create payment via Paymob
            var paymentOutPut = await _paymobService.CreatePaymentAsync(new CreatePaymentDto
            {
                DonorFirstName = donor.FirstName,
                DonorLastName = donor.LastName,
                DonorEmail = donor.Account.Email,
                Amount = model.TotalAmount,
                MerchantOrderId = donation.MerchantOrderId
            });

            donation.PaymentID = paymentOutPut.PaymentId;
            await _unitOfWork.SaveAsync();

            // Return payment link
            return paymentOutPut.PaymentUrl ?? "";
        }
    }


}
