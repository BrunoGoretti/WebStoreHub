namespace WebStoreHubAPI.Dtos
{
    public class DiscountDto
    {
        public int ProductId { get; set; } // Foreign key to the product
        public decimal DiscountPercentage { get; set; } // Optional percentage discount
        public decimal DiscountAmount { get; set; } // Optional fixed discount amount
        public DateTime StartDate { get; set; } // When the discount starts
        public DateTime EndDate { get; set; } // When the discount expires
        public bool IsActive { get; set; } // Indicates if the discount is currently active
    }
}
