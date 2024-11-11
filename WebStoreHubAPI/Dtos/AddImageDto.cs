using WebStoreHubAPI.Models;

namespace WebStoreHubAPI.Dtos
{
    public class AddImageDto
    {
        public int ProductId { get; set; }
        public IFormFile Image { get; set; }
        public ImageMain MainPicture { get; set; }
    }
}
