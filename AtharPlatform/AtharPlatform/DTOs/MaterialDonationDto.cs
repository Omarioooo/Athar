namespace AtharPlatform.DTOs
{
    public class MaterialDonationDTO
    {
        public int Id { get; set; }
        public string DonorFirstName { get; set; } = string.Empty;

        public string DonorLastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public string ItemName { get; set; } = string.Empty;

        public int Quantity { get; set; }
        public string Description { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string MeasurementUnit { get; set; } = string.Empty; // كيلو، قطعة، كرتون، لتر...

        public int MaterialDonationId { get; set; }
    }
}