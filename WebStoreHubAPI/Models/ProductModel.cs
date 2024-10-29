using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStoreHubAPI.Models
{
    public class ProductModel
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; } // Original price
        public decimal? DiscountedPrice { get; set; } // New property for discounted price
        public int Stock { get; set; }
        public string ImageUrl { get; set; }

        public int ProductTypeId { get; set; }
        public int BrandId { get; set; }

        [ForeignKey("ProductTypeId")]
        public ProductTypeModel ProductType { get; set; }

        [ForeignKey("BrandId")]
        public BrandModel Brand { get; set; }
    }
}
