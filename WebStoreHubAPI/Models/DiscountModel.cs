using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStoreHubAPI.Models
{
    public class DiscountModel
    {
        [Key]
        public int DiscountId { get; set; }
        public int ProductId { get; set; } 
        public decimal DiscountPercentage { get; set; }
        public DateTime? StartDate { get; set; } 
        public DateTime? EndDate { get; set; } 
        public bool IsActive { get; set; }
        public string Description { get; set; } 

        public ProductModel Product { get; set; }
    }
}