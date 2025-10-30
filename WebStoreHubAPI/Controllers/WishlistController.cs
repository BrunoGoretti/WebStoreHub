using Microsoft.AspNetCore.Mvc;
using WebStoreHubAPI.Dtos;
using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;

namespace WebStoreHubAPI.Controllers
{
    [Route("api/wishlist")]
    public class WishlistController : Controller
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpPost("addToWishlist")]
        public async Task<IActionResult> AddToWishlist([FromBody] AddToWishlistDto wishlistDto)
        {
            await _wishlistService.AddToWishlist(wishlistDto.UserId, wishlistDto.ProductId);
            return Ok();
        }

        [HttpDelete("removeFromWishlist")]
        public async Task<IActionResult> RemoveFromWishlist(int userId, int productID)
        {
            await _wishlistService.RemoveFromWishlist(userId, productID);
            return Ok();
        }

        [HttpGet("getWishlist")]
        public async Task<ActionResult<List<WishlistItemModel>>> GetWishlist(int userId)
        {
            var wishlistItems = await _wishlistService.GetWishlistItems(userId);
            return Ok(wishlistItems);
        }
    }
}
