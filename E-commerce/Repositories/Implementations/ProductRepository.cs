using System;
using E_commerce.Data;
using E_commerce.Models;
using E_commerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Method to get products with optional filtering and sorting
        public async Task<IEnumerable<Product>> GetAllProductsAsync(decimal? minPrice, decimal? maxPrice, string? categoryName, string? sortBy)
        {
            IQueryable<Product> query = _context.Products.Include(p => p.Category);

            // Apply filtering
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            // Apply category filtering with a null check for Category
            if (!string.IsNullOrEmpty(categoryName))
            {
                query = query.Where(p => p.Category != null && p.Category.Name == categoryName); // Check if Category is not null
            }

            // Apply sorting (optional)
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.ToLower() == "asc")
                {
                    query = query.OrderBy(p => p.Price);
                }
                else if (sortBy.ToLower() == "desc")
                {
                    query = query.OrderByDescending(p => p.Price);
                }
            }

            return await query.ToListAsync();
        }


        // Fetch a product by its ID
        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category) 
                .SingleOrDefaultAsync(p => p.ProductId == id);
            return product;
        }

        // Add a new product
        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        // Update an existing product
        public async Task<Product?> UpdateProductAsync(Product updatedProduct)
        {
            _context.Products.Update(updatedProduct);  
            await _context.SaveChangesAsync();

            // return await _context.Products
            //     .FindAsync(updatedProduct.ProductId); 
            return updatedProduct;
        }


        // Delete a product
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return false; // Product not found, return false
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true; // Product deleted successfully
        }

    }
}
