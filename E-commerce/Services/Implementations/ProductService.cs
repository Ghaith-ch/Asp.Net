using System;
using AutoMapper;
using E_commerce.Dto.ProductDtos;
using E_commerce.Models;
using E_commerce.Repositories.Interfaces;
using E_commerce.Services.Interfaces;

namespace E_commerce.Services.Implementations;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Product> AddProductAsync(CreateProductDto createProductDto)
    {
        if (createProductDto == null)
        {
            throw new ArgumentException("Invalid product data");
        }

        // Map the DTO to a product entity
        var product = _mapper.Map<Product>(createProductDto);

        // Save the product using the repository
        await _productRepository.AddProductAsync(product);

        return product;
    }

    public async Task<Product> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
    {
        if (updateProductDto == null)
        {
            throw new ArgumentException("Invalid product data");
        }

        // Find the existing product by ID
        var existingProduct = await _productRepository.GetProductByIdAsync(id);

        if (existingProduct == null)
        {
            throw new KeyNotFoundException($"Product with ID {id} not found.");
        }

        // Update the product using AutoMapper
        _mapper.Map(updateProductDto, existingProduct);

        // Save the updated product
        var updatedProduct = await _productRepository.UpdateProductAsync(existingProduct);

        return updatedProduct;
    }
}