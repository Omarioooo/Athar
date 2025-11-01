using X.Paymob.CashIn;
using X.Paymob.CashIn.Models.Orders;
using X.Paymob.CashIn.Models.Payment;
using AtharPlatform.DTOs;
using AtharPlatform.Repositories;
using AtharPlatform.Models.Enums;

namespace AtharPlatform.Services
{
    public class PaymobService : IPaymentService<PaymobService>
    {
        private readonly IPaymobCashInBroker _broker;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        public PaymobService(IPaymobCashInBroker broker, IUnitOfWork unitOfWork,
            IConfiguration config)
        {
            _broker = broker;
            _unitOfWork = unitOfWork;
            _config = config;
        }

        public async Task<PaymentOutPutDto> CreatePaymentAsync(CreatePaymentDto model)
        {
            // Create Order
            string MarchentID = Guid.NewGuid().ToString();
            int amountCentes = (int)(model.Amount * 100);
            var orderRequest = CashInCreateOrderRequest.CreateOrder(amountCentes, MarchentID);
            var order = await _broker.CreateOrderAsync(orderRequest);

            // Create Billing
            var billingData = new CashInBillingData(
             firstName: model.DonorFirstName,
             lastName: model.DonorLastName,
             email: model.DonorEmail,
             phoneNumber: "",
             country: "EG",
             state: "",
             city: "Alexandria",
             apartment: "",
             street: "Street",
             floor: "",
             building: "",
             shippingMethod: "",
             postalCode: ""
            );

            // Create Payment
            var paymentKeyRequest = new CashInPaymentKeyRequest(
                 integrationId: int.Parse(_config["Paymob:IntegrationId"]),
                 orderId: order.Id,
                 billingData: billingData,
                 amountCents: amountCentes,
                 currency: "EGP",
                 lockOrderWhenPaid: false,
                 expiration: null
             );

            var paymentKey = await _broker.RequestPaymentKeyAsync(paymentKeyRequest);

            // IFrame URL
            string iframeId = _config["Paymob:IframeId"];
            string paymentUrl = $"https://accept.paymob.com/api/acceptance/iframes/{iframeId}?payment_token={paymentKey.PaymentKey}";

            return new PaymentOutPutDto()
            {
                PaymentUrl = paymentUrl,
                PaymentId = order.Id.ToString()
            };
        }

        public async Task<bool> VerifyPaymentAsync(VerifyPaymentDto model)
        {
            // get the payment saved on paymob
            var transaction = await _broker.GetTransactionAsync(model.TransactionId);
            if (transaction == null)
                return false;

            // get the payment saved on server
            var donation = _unitOfWork.Donations.FirstOrDefault(d => d.MerchantOrderId == model.DonationMerchantOrderId);
            if (donation == null)
                return false;

            if (transaction.Success)
            {
                donation.DonationStatus = TransactionStatusEnum.SUCCESSED;
                await _unitOfWork.SaveAsync();
                return true;
            }

            donation.DonationStatus = TransactionStatusEnum.FAILED;
            await _unitOfWork.SaveAsync();
            return false;
        }

        public async Task<PaymentDetailsDto> GetPaymentDetailsAsync(string TransactionId)
        {
            var transaction = await _broker.GetTransactionAsync(TransactionId);
            if (transaction == null)
                throw new Exception("Transaction not found.");

            return new PaymentDetailsDto
            {
                PaymobTransactionId = transaction.Id.ToString(),
                DonationMerchantOrderId = transaction.Order?.MerchantOrderId,
                Amount = transaction.AmountCents / 100m,
                Currency = transaction.Currency,
                PaidAt = transaction.CreatedAt.DateTime,
                DonorName = $"{transaction.BillingData.FirstName} {transaction.BillingData.LastName}",
                DonorEmail = transaction.BillingData.Email
            };
        }
        //public async Task HandleWebhookAsync(PaymobWebhookDto payload)
        //{
        //    try
        //    {
        //        var data = payload.Obj;
        //        var merchantOrderId = data.MerchantOrderId;
        //        var transactionId = data.TransactionId;
        //        var success = data.Success;

        //        // fint the donation
        //        var donation = await _unitOfWork
        //            .Donations
        //            .FirstOrDefaultAsync(d => d.MerchantOrderId == merchantOrderId);

        //        if (donation == null)
        //            return;


        //        // 2. حدث الحالة

        //        donation.TransactionId = transactionId;

        //        donation.DonationStatus = success

        //            ? TransactionStatusEnum.SUCCESSED

        //            : TransactionStatusEnum.FAILED;

        //        // 3. لو الدفع نجح، زوّد رصيد الجمعية

        //        if (success)

        //        {

        //            var charityDonation = await _unitOfWork.CharityDonations

        //                .FirstOrDefaultAsync(cd => cd.DonationId == donation.Id);

        //            if (charityDonation != null)

        //            {

        //                var charity = await _unitOfWork.Charity.GetAsync(charityDonation.charityID);

        //                charity.Balance += donation.NetAmountToCharity;

        //                await _unitOfWork.Charity.UpdateAsync(charity);

        //            }

        //        }

        //        await _unitOfWork.SaveAsync();

        //    }

        //    catch (Exception ex)

        //    {




        //    }

        //}

        public Task HandleWebHookAsync(string paymentServiceId)
        {
            throw new NotImplementedException();
        }
    }
}
