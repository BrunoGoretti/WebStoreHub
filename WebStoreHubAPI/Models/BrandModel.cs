using System.ComponentModel.DataAnnotations;

namespace WebStoreHubAPI.Models
{
    public class BrandModel
    {
        [Key]
        public int BrandId { get; set; }
        public string BrandName { get; set; }
    }
}
