using AutoMapper;
using E_commerce.Dto.OrderDtos;
using E_commerce.Models;
using E_commerce.Models.Enums;
using E_commerce.Repositories.Interfaces;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace E_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IOrderRepository orderRepository, IMapper mapper)
        {
            _orderService = orderService;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GetOrderDto>>> GetOrdersForAuthenticatedUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "User ID not found in token." });
            }

            // Retrieve orders
            var orders = await _orderRepository.GetOrdersByUserIdAsync(int.Parse(userId));
            if (orders == null || !orders.Any())
            {
                return NotFound(new { Message = "No orders found for this user." });
            }

            var orderDtos = _mapper.Map<List<GetOrderDto>>(orders);
            return Ok(orderDtos);
        }


        // GET: api/Order/{orderId}
        [HttpGet("{orderId}")]
        public async Task<ActionResult<GetOrderDto>> GetOrderById(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound(new { Message = "Order not found." });
            }

            var orderDto = _mapper.Map<GetOrderDto>(order);
            return Ok(orderDto);
        }

        // POST: api/Order
        [HttpPost]
        public async Task<ActionResult<GetOrderDto>> CreateOrder()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "User ID not found in token." });
            }

            try
            {
                var order = await _orderService.CreateOrderAsync(int.Parse(userId));
                var orderDto = _mapper.Map<GetOrderDto>(order);
                return CreatedAtAction(nameof(GetOrderById), new { orderId = order.OrderId }, orderDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PATCH: api/Order/{orderId}/status
        // PATCH: api/Order/{orderId}/status
        [Authorize(Roles = "Admin")]
        [HttpPatch("{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromForm] string status)
        {
            if (!Enum.TryParse<OrderStatus>(status, true, out var orderStatus))
            {
                return BadRequest(new { Message = $"Invalid order status: {status}" });
            }

            try
            {
                var updatedOrder = await _orderService.UpdateOrderStatusAsync(orderId, orderStatus);
                var orderDto = _mapper.Map<GetOrderDto>(updatedOrder);
                return Ok(new { Message = "Order status updated successfully.", Order = orderDto });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }


        // DELETE: api/Order/{orderId}
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            try
            {
                await _orderService.DeleteOrderAsync(orderId);
                return Ok(new { Message = "Order deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}
