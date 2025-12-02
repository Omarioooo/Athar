
namespace AtharPlatform.DTOs
{
    public class DonorProfileDto
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string Email { get; set; }

        public string? ImageUrl { get; set; }

        public string? Country { get; set; }

        public string? City { get; set; }

      
        public int DonationsCount { get; set; } = 0;
        public int FollowingCount { get; set; } = 0;

        public double TotalDonationsAmount { get; set; } = 0;

        //لي هنعمل كده 
        //لو سيستيم الـ frontend بيحاول يقرأ DonationsHistory، لو كانت field مش property، هتطلع null → NullReferenceException.
        public List<DonationsHistoryDto> DonationsHistory { get; set; } = new();

    }
}
