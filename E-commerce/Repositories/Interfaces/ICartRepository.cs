using System;
using System.Threading.Tasks;
using E_commerce.Models;

namespace E_commerce.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetCartByUserIdAsync(int userId);
        Task<Cart?> GetCartByIdAsync(int cartId);
        Task<Cart> AddCartAsync(Cart cart);
        Task<bool> RemoveCartAsync(int cartId);
        Task<CartItem> AddCartItemAsync(CartItem cartItem);
        Task<bool> RemoveCartItemAsync(int cartItemId);
        Task<Product?> GetProductByIdAsync(int productId);
        
        Task<ApplicationUser?> GetUserByIdAsync(int userId);
    }
}
