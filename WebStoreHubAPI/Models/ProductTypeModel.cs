using System.ComponentModel.DataAnnotations;

namespace WebStoreHubAPI.Models
{
    public class ProductTypeModel
    {
        [Key]
        public int ProductTypeId { get; set; }
        public string TypeName { get; set; }
    }
}
