using System;
using E_commerce.Models;
using E_commerce.Models.Enums;

namespace E_commerce.Services.Interfaces;

public interface IOrderService
{
        Task<Order> CreateOrderAsync(int userId);
        Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<bool> DeleteOrderAsync(int orderId);
}