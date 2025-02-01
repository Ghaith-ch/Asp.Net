using System;
using E_commerce.Models;

namespace E_commerce.Repositories.Interfaces;

public interface IProductRepository
{
    // Method to get products with optional filtering and sorting
    Task<IEnumerable<Product>> GetAllProductsAsync(decimal? minPrice, decimal? maxPrice, string? categoryName, string? sortBy);


    Task<Product?> GetProductByIdAsync(int id);
    Task AddProductAsync(Product product);
    Task<Product?> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(int id);
}
