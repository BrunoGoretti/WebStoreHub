using Microsoft.AspNetCore.Mvc;
using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;

namespace WebStoreHubAPI.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(int userId, int productId, int quantity)
        {
            await _cartService.AddToCart(userId, productId, quantity);
            return Ok();
        }

        [HttpPut("update/{cartItemId}")]
        public async Task<IActionResult> UpdateCartItem(int userId, int cartItemId, int quantity)
        {
            await _cartService.UpdateCartItem(userId, cartItemId, quantity);
            return Ok();
        }

        [HttpDelete("remove/{cartItemId}")]
        public async Task<IActionResult> RemoveFromCart(int userId, int cartItemId)
        {
            await _cartService.RemoveFromCart(userId, cartItemId);
            return Ok();
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart(int userId)
        {
            await _cartService.ClearCart(userId);
            return Ok();
        }

        [HttpGet("items")]
        public async Task<ActionResult<List<CartItemModel>>> GetCartItems(int userId)
        {
            var cartItems = await _cartService.GetCartItems(userId);
            return Ok(cartItems);
        }

    }
}
