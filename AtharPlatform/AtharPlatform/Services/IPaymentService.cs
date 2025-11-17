using AtharPlatform.DTO;
using AtharPlatform.Models.Enums;

namespace AtharPlatform.Services
{
    public interface IPaymentService
    {
        Task<PaymentOutPutDto> CreatePaymentAsync(CreatePaymentDto model);
        Task HandleWebHookAsync(Dictionary<string, string> data);
        bool VerifyHmac(Dictionary<string, string> data, string receivedHmac);

        // Methods you missed:
        Task<string> AuthenticateAsync();
        Task<int> CreateOrderAsync(decimal amountCents, string merchantOrderId);
        Task<string> CreatePaymentKeyAsync(int orderId, decimal amountCents);
        Task<bool> VerifyPaymentAsync(VerifyPaymentDto model);
        Task<PaymentDetailsDto> GetPaymentDetailsAsync(string transactionId);
    }

}