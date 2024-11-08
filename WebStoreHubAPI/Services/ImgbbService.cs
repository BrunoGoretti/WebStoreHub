using WebStoreHubAPI.Data;
using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;

namespace WebStoreHubAPI.Services
{
    public class ImgbbService : IImgbbService
    {
        private readonly AppDbContext _context;

        public ImgbbService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ImgbbModel> AddImageAsync(ImgbbModel image)
        {
            _context.DbImgbbImages.Add(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task<bool> RemoveImageAsync(int imageId)
        {
            var image = await _context.DbImgbbImages.FindAsync(imageId);
            if (image == null)
            {
                return false;
            }

            _context.DbImgbbImages.Remove(image);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}