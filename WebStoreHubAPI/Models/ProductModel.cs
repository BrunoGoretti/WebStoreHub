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
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }

        // Foreign key for ProductType
        public int ProductTypeId { get; set; }

        [ForeignKey("ProductTypeId")]
        public ProductTypeModel ProductType { get; set; }
    }
}
