using WebStoreHubAPI.Models;

namespace WebStoreHubAPI.Dtos
{
    public class ImgbbResponseDto
    {
        public int ImageId { get; set; }
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public ImageMain MainPicture { get; set; }
    }
}
