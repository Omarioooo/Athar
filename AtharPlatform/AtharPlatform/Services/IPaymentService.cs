using AtharPlatform.DTOs;
using AtharPlatform.Models.Enums;

namespace AtharPlatform.Services
{
    public interface IPaymentService<T> where T : class
    {
        Task<PaymentOutPutDto> CreatePaymentAsync(CreatePaymentDto model);
        Task HandleWebHookAsync(string paymentServiceId);

    }
}