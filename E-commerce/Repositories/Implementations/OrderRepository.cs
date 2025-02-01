using System;
using E_commerce.Data;
using E_commerce.Models;
using E_commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create a new order
        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        // Get an order by its ID
        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems) // Include OrderItems
                .ThenInclude(oi => oi.Product) // Include Product details for each OrderItem
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        // Get all orders for a specific user
        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems) // Include OrderItems
                .ThenInclude(oi => oi.Product) // Include Product details for each OrderItem
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }


        // Delete an order
        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order == null)
            {
                return false;
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        // Get a user by ID (new method)
        public async Task<ApplicationUser?> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Orders) // Include Orders (optional)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
