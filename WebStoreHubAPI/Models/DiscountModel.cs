using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStoreHubAPI.Models
{
    public class DiscountModel
    {
        [Key]
        public int DiscountId { get; set; } // Unique identifier for the discount
        public int ProductId { get; set; } // Foreign key to the ProductModel
        public decimal DiscountPercentage { get; set; } // Percentage discount
        public decimal DiscountAmount { get; set; } // Fixed amount discount
        public DateTime? StartDate { get; set; } // When the discount becomes active
        public DateTime? EndDate { get; set; } // When the discount expires
        public bool IsActive { get; set; } // Indicates if the discount is currently active
        public string Description { get; set; } // Optional description of the discount

        // Navigation property to relate the discount to the product
        public ProductModel Product { get; set; }
    }
}