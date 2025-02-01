using AutoMapper;
using E_commerce.Models;
using E_commerce.Models.Enums;
using E_commerce.Repositories.Interfaces;
using E_commerce.Services.Interfaces;

namespace E_commerce.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<Order> CreateOrderAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null || !cart.CartItems.Any())
            {
                throw new InvalidOperationException("Cart is empty or not found for the user.");
            }

            var user = await _orderRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            var totalPrice = cart.CartItems.Sum(item => item.UnitPrice * item.Quantity);

            var orderItems = cart.CartItems.Select(item => _mapper.Map<OrderItem>(item)).ToList();
            var order = new Order
            {
                UserId = userId,
                User = user,
                TotalAmount = totalPrice,
                OrderDate = DateTime.UtcNow,
                OrderItems = orderItems,
                Status = OrderStatus.Pending // Explicitly set default status
            };

            await _orderRepository.CreateOrderAsync(order);
            await _cartRepository.RemoveCartAsync(cart.CartId);

            return order;
        }


        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }

        public async Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException("Order not found.");
            }

            order.Status = status;
            return await _orderRepository.UpdateOrderAsync(order);
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException("Order not found.");
            }

            return await _orderRepository.DeleteOrderAsync(orderId);
        }
    }
}
