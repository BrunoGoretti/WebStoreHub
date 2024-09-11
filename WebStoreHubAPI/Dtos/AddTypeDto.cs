using System.ComponentModel.DataAnnotations;

namespace WebStoreHubAPI.Dtos
{
    public class AddTypeDto
    {
        [Required]
        public string TypeName { get; set; }
    }
}
