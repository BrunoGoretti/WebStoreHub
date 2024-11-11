using WebStoreHubAPI.Models;

namespace WebStoreHubAPI.Services.Interfaces
{
    public interface IImgbbService
    {
        Task<ImgbbModel> AddImageAsync(IFormFile imageFile, int productId, ImageMain mainPicture);
        Task<bool> RemoveImageAsync(int imageId);

    }
}
