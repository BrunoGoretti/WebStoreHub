/*using Microsoft.AspNetCore.Mvc;
using WebStoreHubAPI.Services.Interfaces;

namespace WebStoreHubAPI.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder(int userId, decimal totalAmount)
        {
            var order = await _orderService.CreateOrderAsync(userId, totalAmount);
            return Ok(order);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(int userId)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        [HttpPut("updateStatus")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, string status)
        {
            var result = await _orderService.UpdateOrderStatusAsync(orderId, status);
            if (!result)
            {
                return NotFound();
            }

            return Ok("Order status updated");
        }

        [HttpDelete("delete/{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var result = await _orderService.DeleteOrderAsync(orderId);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
*/