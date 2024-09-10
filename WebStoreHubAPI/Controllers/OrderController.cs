using Microsoft.AspNetCore.Mvc;
using WebStoreHubAPI.Services.Interfaces;

namespace WebStoreHubAPI.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;

        public OrderController(IOrderService orderService, ICartService cartService)
        {
            _orderService = orderService;
            _cartService = cartService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder(int userId)
        {
            var cartItems = await _cartService.GetCartItems(userId);
            if (cartItems == null || !cartItems.Any())
            {
                return BadRequest("No items in the cart.");
            }

            var order = await _orderService.CreateOrderAsync(userId, cartItems);
            return Ok(order);
        }

        [HttpGet("orders/{id}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound("Order not found.");
            }
            return Ok(order);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(int userId)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        [HttpPut("{orderId}/updateStatus")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] string newStatus)
        {
            var success = await _orderService.UpdateOrderStatusAsync(orderId, newStatus);
            if (!success)
            {
                return NotFound("Order not found.");
            }
            return Ok("Order status updated.");
        }
    }
}
