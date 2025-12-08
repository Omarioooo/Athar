using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using AtharPlatform.DTO;
using AtharPlatform.Models;
using AtharPlatform.Models.Enums;
using AtharPlatform.Repositories;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AtharPlatform.Services
{
    public class PaymentSettings
    {
        public string ApiKey { get; set; } = null!;
        public string IntegrationId { get; set; } = null!;
        public string IFrameId { get; set; } = null!;
        public string Hmac { get; set; } = null!;
    }

    public class PaymobService : IPaymentService
    {
        private readonly HttpClient _http;
        private readonly PaymentSettings _settings;
        private readonly IUnitOfWork _unit;

        public PaymobService(IHttpClientFactory httpFactory, IOptions<PaymentSettings> settings, IUnitOfWork unit)
        {
            _http = httpFactory.CreateClient();
            _settings = settings.Value;
            _unit = unit;
        }

        // =================== Internal Paymob Calls ===================
        private async Task<string> GetAuthToken()
        {
            var body = new { api_key = _settings.ApiKey };
            var response = await _http.PostAsync(
                "https://accept.paymob.com/api/auth/tokens",
                new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
            );
            var json = await response.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(json);
            return data.token;
        }

        private async Task<int> CreateOrder(string token, int amountCents, string merchantOrderId)
        {
            var body = new
            {
                auth_token = token,
                amount_cents = amountCents,
                delivery_needed = "false",
                currency = "EGP",
                merchant_order_id = merchantOrderId,
                items = new object[] { }
            };

            var response = await _http.PostAsync(
                "https://accept.paymob.com/api/ecommerce/orders",
                new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
            );
            var json = await response.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(json);
            return data.id;
        }

        private async Task<string> CreatePaymentKey(string token, int orderId, int amountCents, string email)
        {
            var billingData = new
            {
                apartment = "NA",
                email = email,
                floor = "NA",
                first_name = "User",
                last_name = "Donor",
                phone_number = "0000000000",
                city = "Cairo",
                country = "EG",
                street = "NA",
                building = "NA"
            };

            var body = new
            {
                auth_token = token,
                amount_cents = amountCents,
                expiration = 3600,
                order_id = orderId,
                billing_data = billingData,
                currency = "EGP",
                integration_id = int.Parse(_settings.IntegrationId)
            };

            var response = await _http.PostAsync(
                "https://accept.paymob.com/api/acceptance/payment_keys",
                new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json")
            );

            var json = await response.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(json);
            return data.token;
        }

        //نسخه قديمة
        // =================== Payment Creation ===================
        //public async Task<PaymentOutPutDto> CreatePaymentAsync(CreatePaymentDto model)
        //{
        //    string merchantOrderId = Guid.NewGuid().ToString();

        //    int amountCents = (int)(model.Amount * 100);

        //    // 1) Create Donation in DB First
        //    var donation = new Donation
        //    {
        //        DonorId = model.DonorId,
        //        TotalAmount = model.Amount,
        //        NetAmountToCharity = model.Amount * 0.98m,
        //        DonationStatus = TransactionStatusEnum.PENDING,
        //        MerchantOrderId = merchantOrderId,
        //        CharityId = model.CharityId,
        //        CreatedAt = DateTime.UtcNow,
        //        CampaignId=model.CampaignId
        //    };

        //    await _unit.PaymentDonations.AddAsync(donation);
        //    await _unit.SaveAsync();

        //    // 2) Link to Campaign if exists
        //    if (model.CampaignId>0)
        //    {
        //        await _unit.PaymentCampaignDonations.AddAsync(new CampaignDonation
        //        {
        //            DonationId = donation.Id,
        //            CampaignId = model.CampaignId,
        //            DonorId=model.DonorId
        //        });
        //        await _unit.SaveAsync();
        //    }

        //    // 3) Paymob Auth & Order
        //    string token = await GetAuthToken();
        //    int orderId = await CreateOrder(token, amountCents, merchantOrderId);

        //    donation.PaymentID = orderId.ToString();
        //    await _unit.PaymentDonations.UpdateAsync(donation);
        //    await _unit.SaveAsync();

        //    // 4) Generate Payment Key
        //    string paymentKey = await CreatePaymentKey(token, orderId, amountCents, model.DonorEmail);

        //    string paymentUrl =
        //        $"https://accept.paymob.com/api/acceptance/iframes/{_settings.IFrameId}?payment_token={paymentKey}";

        //    // ---- FIXED: Correct return values ----
        //    return new PaymentOutPutDto
        //    {
        //        PaymentId = orderId.ToString(),
        //        MerchantOrderId = merchantOrderId,
        //        PaymentUrl = paymentUrl
        //    };
        //}


        // =================== HMAC Verification ===================
        public async Task<PaymentOutPutDto> CreatePaymentAsync(CreatePaymentDto model)
        {
            // 1️⃣ تأكيد وجود الدونور قبل ما تعمل أي Insert
            var donorExists = await _unit.Donors.ExistsAsync(model.DonorId);
            if (!donorExists)
                throw new Exception("Donor does not exist");

            string merchantOrderId = Guid.NewGuid().ToString();
            int amountCents = (int)(model.Amount * 100);

            
            var donation = new Donation
            {
                DonorId = model.DonorId,
                CharityId = model.CharityId,
                CampaignId = model.CampaignId,
                TotalAmount = model.Amount,
                NetAmountToCharity = model.Amount * 0.98m,
                DonationStatus = TransactionStatusEnum.PENDING,
                MerchantOrderId = merchantOrderId,
                CreatedAt = DateTime.UtcNow
            };

            await _unit.PaymentDonations.AddAsync(donation);
            await _unit.SaveAsync(); // UnitOfWork بيحفظ

           
            string token = await GetAuthToken();
            int orderId = await CreateOrder(token, amountCents, merchantOrderId);

            donation.PaymentID = orderId.ToString();
            await _unit.PaymentDonations.UpdateAsync(donation);
            await _unit.SaveAsync();

            // 4️⃣ Get Payment Key
            string paymentKey = await CreatePaymentKey(token, orderId, amountCents, model.DonorEmail);

            string paymentUrl =
                $"https://accept.paymob.com/api/acceptance/iframes/{_settings.IFrameId}?payment_token={paymentKey}";

            return new PaymentOutPutDto
            {
                DonationId = donation.Id,
                PaymentId = orderId.ToString(),
                MerchantOrderId = merchantOrderId,
                PaymentUrl = paymentUrl
            };
        }

        public bool VerifyHmac(Dictionary<string, string> data, string receivedHmac)
        {
            var ordered = data
                .Where(k => k.Key != "hmac")
                .OrderBy(x => x.Key)
                .Select(x => x.Value)
                .Aggregate("", (acc, val) => acc + val);

            using var h = new HMACSHA512(Encoding.UTF8.GetBytes(_settings.Hmac));
            var hash = BitConverter.ToString(h.ComputeHash(Encoding.UTF8.GetBytes(ordered)))
                .Replace("-", "")
                .ToLower();

            return hash == receivedHmac.ToLower();
        }

        // =================== Webhook Handling ===================
        public async Task HandleWebHookAsync(Dictionary<string, string> data)
        {
            //string orderId = data["order"];
            string merchantOrderId = data["order_merchant_order_id"];
            bool success = data["success"].ToLower() == "true";
            string transactionId = data["id"];

            var donation = await _unit.PaymentDonations.GetWithExpressionAsync(d => d.MerchantOrderId == merchantOrderId);
            if (donation == null) return;

            donation.TransactionId = transactionId;
            donation.DonationStatus = success ? TransactionStatusEnum.SUCCESSED : TransactionStatusEnum.FAILED;
            await _unit.PaymentDonations.UpdateAsync(donation);

            if (!success)
            {
                await _unit.SaveAsync();
                return;
            }

            var campaignDonation = await _unit.PaymentCampaignDonations.GetWithExpressionAsync(cd => cd.DonationId == donation.Id);
            if (campaignDonation != null)
            {
                var campaign = await _unit.Campaigns.GetAsync(campaignDonation.CampaignId);
                if (campaign != null)
                {
                    campaign.RaisedAmount +=(double) donation.TotalAmount;
                    await _unit.Campaigns.UpdateAsync(campaign);

                    var charity = await _unit.Charities.GetAsync(campaign.CharityID);
                    if (charity != null)
                    {
                        charity.Balance += donation.NetAmountToCharity;
                        await _unit.Charities.UpdateAsync(charity);
                    }
                }
            }

            await _unit.SaveAsync();
        }

        // =================== Interface Required Methods ===================
        public async Task<string> AuthenticateAsync() => await GetAuthToken();

        public async Task<int> CreateOrderAsync(decimal amountCents, string merchantOrderId)
            => await CreateOrder(await GetAuthToken(), (int)amountCents, merchantOrderId);

        public async Task<string> CreatePaymentKeyAsync(int orderId, decimal amountCents)
            => await CreatePaymentKey(await GetAuthToken(), orderId, (int)amountCents, "user@example.com");

        public async Task<bool> VerifyPaymentAsync(VerifyPaymentDto model)
        {
            var response = await _http.GetAsync($"https://accept.paymob.com/api/acceptance/transactions/{model.TransactionId}");
            var json = await response.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(json);

            bool success = data.success == true;

            var donation = await _unit.PaymentDonations.GetWithExpressionAsync(d => d.MerchantOrderId == model.DonationMerchantOrderId);
            if (donation == null) return false;

            donation.DonationStatus = success ? TransactionStatusEnum.SUCCESSED : TransactionStatusEnum.FAILED;
            donation.TransactionId = model.TransactionId;

            await _unit.PaymentDonations.UpdateAsync(donation);
            await _unit.SaveAsync();

            return success;
        }

        public async Task<PaymentDetailsDto> GetPaymentDetailsAsync(string transactionId)
        {
            var response = await _http.GetAsync($"https://accept.paymob.com/api/acceptance/transactions/{transactionId}");
            var json = await response.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(json);

            return new PaymentDetailsDto
            {
                PaymobTransactionId = transactionId,
                DonationMerchantOrderId = data.order?.merchant_order_id,
                Amount = (decimal)data.amount_cents / 100m,
                Currency = data.currency,
                PaidAt = data.created_at,
                DonorName = data.billing_data?.first_name + " " + data.billing_data?.last_name,
                DonorEmail = data.billing_data?.email
            };
        }


        public async Task<decimal> GetTotalDonationsForCharityAsync(int charityId)
        {
            return await _unit.PaymentDonations
                .GetAll()
                .Where(d => d.CharityId == charityId
                            && d.DonationStatus == TransactionStatusEnum.SUCCESSED)
                .SumAsync(d => d.NetAmountToCharity);
        }
    }
}
