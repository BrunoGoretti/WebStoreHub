using Microsoft.AspNetCore.Mvc;
using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;

namespace WebStoreHubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImgbbController : ControllerBase
    {
        private readonly IImgbbService _imgbbService;

        public ImgbbController(IImgbbService imgbbService)
        {
            _imgbbService = imgbbService;
        }

        [HttpPost("AddImage")]
        public async Task<IActionResult> AddImage()
        {
   
        }

        [HttpDelete("RemoveImage/{imageId}")]
        public async Task<IActionResult> RemoveImage(int imageId)
        {
            var result = await _imgbbService.RemoveImageAsync(imageId);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
