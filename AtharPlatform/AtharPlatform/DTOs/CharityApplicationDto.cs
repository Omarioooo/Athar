namespace AtharPlatform.DTOs
{
    public class CharityApplicationResponseDto
    {
        public int Id { get; set; }
        public string Type { get; set; }  // Volunteer / VendorOffer

        // Common
        public string Name { get; set; }
        public string Phone { get; set; }
       //public string Country { get; set; }
       // public string City { get; set; }

        // Vendor 
        /// <summary>
        /// public string? ItemName { get; set; }
        /// </summary>
       /// public int? Quantity { get; set; }
       // public decimal? PriceBefore { get; set; }
       // public decimal? PriceAfter { get; set; }

        // Volunteer 
       // public int? Age { get; set; }
       // public bool? IsFirstTime { get; set; }


        public DateTime? Date { get; set; }
        public string? Description { get; set; }

       /// public string? MeasurementUnit{ get; set; }
    }
}
