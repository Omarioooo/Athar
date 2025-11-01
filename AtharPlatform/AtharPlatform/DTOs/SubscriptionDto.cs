using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AtharPlatform.DTOs
{
    public class SubscriptionDto
    {
        public int DonorID { get; set; }
        public int CharityID { get; set; }
        public decimal Amount { get; set; } = 0;
    }
}