using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PdfSharp.Capabilities.Features;

namespace WebStoreHubAPI.Models
{
    public class ProductModel
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; } 
        public int Stock { get; set; }

        public int ProductTypeId { get; set; }
        public int BrandId { get; set; }

        [ForeignKey("ProductTypeId")]
        public ProductTypeModel ProductType { get; set; }

        [ForeignKey("BrandId")]
        public BrandModel Brand { get; set; }
        public List<ImgbbModel> Images { get; set; }

        public List<DiscountModel> Discounts { get; set; }
    }
}
