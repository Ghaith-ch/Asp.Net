using System;
using E_commerce.Models;

namespace E_commerce.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order); // To create a new order
        Task<Order?> GetOrderByIdAsync(int orderId); // To get an order by its ID
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId); // To get all orders for a user
        Task<Order> UpdateOrderAsync(Order order); // To update an order (e.g., for changing status)
        Task<bool> DeleteOrderAsync(int orderId); // To delete an order

        Task<ApplicationUser?> GetUserByIdAsync(int userId); // New method to get a user by their ID
    }
}
