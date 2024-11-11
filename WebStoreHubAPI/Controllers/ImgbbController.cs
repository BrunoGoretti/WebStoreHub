using Microsoft.AspNetCore.Mvc;
using WebStoreHubAPI.Dtos;
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
        public async Task<IActionResult> AddImage([FromForm] AddImageDto addImageDto)
        {
            if (addImageDto.Image == null)
            {
                return BadRequest("Image file is required.");
            }

            var addedImage = await _imgbbService.AddImageAsync(addImageDto.Image, addImageDto.ProductId, addImageDto.MainPicture);

            var responseDto = new ImgbbResponseDto
            {
                ImageId = addedImage.ImageId,
                ProductId = addedImage.ProductId,
                ImageUrl = addedImage.ImageUrl,
                MainPicture = addedImage.MainPicture
            };

            return CreatedAtAction(nameof(AddImage), new { id = responseDto.ImageId }, responseDto);
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
