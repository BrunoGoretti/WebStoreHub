using System.ComponentModel.DataAnnotations;

namespace WebStoreHubAPI.Dtos
{
    public class AddBrandDto
    {
        [Required]
        public string BrandName { get; set; }
    }
}
