using WebStoreHubAPI.Models;

namespace WebStoreHubAPI.Services.Interfaces
{
    public interface IImgbbService
    {
        Task<ImgbbModel> AddImageAsync(ImgbbModel image);
        Task<bool> RemoveImageAsync(int imageId);
    }
}
