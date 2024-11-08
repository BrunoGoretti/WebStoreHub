using System.ComponentModel.DataAnnotations;

namespace WebStoreHubAPI.Models
{
    public class ImgbbModel
    {
        [Key]
        public int ImageId { get; set; } 

        public int ProductId { get; set; } 

        public string ImageUrl { get; set; } 

        public ImageMain MainPicture { get; set; }
    }

    public enum ImageMain
    {
        Secondary = 0,
        Primary = 1
    }
}
