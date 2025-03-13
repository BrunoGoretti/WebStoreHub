using System.Text.Json;
using WebStoreHubAPI.Data;
using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;

namespace WebStoreHubAPI.Services
{
    public class ImgbbService : IImgbbService
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ImgbbService(AppDbContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<ImgbbModel> AddImageAsync(IFormFile imageFile, int productId, ImageMain mainPicture)
        {
            var imageUrl = await UploadToImgbbAsync(imageFile);
            if (imageUrl == null) throw new Exception("Failed to upload image to Imgbb");

            var image = new ImgbbModel
            {
                ProductId = productId,
                ImageUrl = imageUrl,
                MainPicture = mainPicture
            };

            _context.DbImgbbImages.Add(image);
            await _context.SaveChangesAsync();

            return image;
        }

        public async Task<string> UploadToImgbbAsync(IFormFile imageFile)
        {
            var apiKey = _configuration["ImgbbSettings:ApiKey"];
            var uploadUrl = _configuration["ImgbbSettings:UploadUrl"];
            var client = _httpClientFactory.CreateClient();

            using var content = new MultipartFormDataContent();
            using var ms = new MemoryStream();
            await imageFile.CopyToAsync(ms);
            var fileBytes = ms.ToArray();
            content.Add(new ByteArrayContent(fileBytes), "image", imageFile.FileName);
            content.Add(new StringContent(apiKey), "key");

            var response = await client.PostAsync(uploadUrl, content);
            if (!response.IsSuccessStatusCode) return null;

            var responseContent = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(responseContent);
            var imageUrl = json.RootElement.GetProperty("data").GetProperty("url").GetString();
            return imageUrl;
        }

        public async Task<bool> RemoveImageAsync(int imageId)
        {
            var image = await _context.DbImgbbImages.FindAsync(imageId);
            if (image == null) return false;

            _context.DbImgbbImages.Remove(image);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
