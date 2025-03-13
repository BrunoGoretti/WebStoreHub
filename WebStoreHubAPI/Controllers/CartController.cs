using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStoreHubAPI.Dtos;
using WebStoreHubAPI.Models;
using WebStoreHubAPI.Services.Interfaces;

namespace WebStoreHubAPI.Controllers
{
    [Route("api/cart")]
    // [Authorize(Roles = "Admin, User")]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("addToCart")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto cartDto)
        {
            if (cartDto == null)
            {
                return BadRequest("Invalid request data.");
            }

            await _cartService.AddToCart(cartDto.UserId, cartDto.ProductId, cartDto.Quantity);
            return Ok();
        }

        [HttpPut("updateCart/{cartItemId}")]
        public async Task<IActionResult> UpdateCartItem(int userId, int cartItemId, int quantity)
        {
            await _cartService.UpdateCartItem(userId, cartItemId, quantity);
            return Ok();
        }

        [HttpDelete("removeFromCart/{cartItemId}")]
        public async Task<IActionResult> RemoveFromCart(int userId, int cartItemId)
        {
            await _cartService.RemoveFromCart(userId, cartItemId);
            return Ok();
        }

        [HttpDelete("clearCart")]
        public async Task<IActionResult> ClearCart(int userId)
        {
            await _cartService.ClearCart(userId);
            return Ok();
        }

        [HttpGet("cartItems")]
        public async Task<ActionResult<List<CartItemModel>>> GetCartItems(int userId)
        {
            var cartItems = await _cartService.GetCartItems(userId);
            return Ok(cartItems);
        }

    }
}
