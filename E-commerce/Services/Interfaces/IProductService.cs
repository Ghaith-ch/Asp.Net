using System;
using E_commerce.Dto.ProductDtos;
using E_commerce.Models;

namespace E_commerce.Services.Interfaces;

public interface IProductService
{
        Task<Product> AddProductAsync(CreateProductDto createProductDto);
        Task<Product> UpdateProductAsync(int id, UpdateProductDto updateProductDto);
}
