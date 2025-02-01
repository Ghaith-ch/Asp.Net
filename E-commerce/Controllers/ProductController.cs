using AutoMapper;
using E_commerce.Dto.ProductDtos;
using E_commerce.Models;
using E_commerce.Repositories.Interfaces;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IProductRepository productRepository, IMapper mapper)
        {
            _productService = productService;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetProductDto>>> GetProducts(
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] string? categoryName = null,
            [FromQuery] string? sortBy = null)
        {
            var products = await _productRepository.GetAllProductsAsync(minPrice, maxPrice, categoryName, sortBy);

            if (products == null || !products.Any())
            {
                return NotFound(new { Message = "No products found." });
            }

            var productsDto = _mapper.Map<IEnumerable<GetProductDto>>(products);
            return Ok(productsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound(new { Message = $"Product with ID {id} not found." });
            }

            var productDto = _mapper.Map<GetProductDto>(product);
            return Ok(productDto);
        }

        [HttpPost]
        public async Task<ActionResult<GetProductDto>> AddProduct([FromForm] CreateProductDto createProductDto)
        {
            try
            {
                var product = await _productService.AddProductAsync(createProductDto);
                var productDto = _mapper.Map<GetProductDto>(product);
                return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, productDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<GetProductDto>> UpdateProduct(int id, [FromForm] UpdateProductDto updateProductDto)
        {
            try
            {
                var updatedProduct = await _productService.UpdateProductAsync(id, updateProductDto);
                var productDto = _mapper.Map<GetProductDto>(updatedProduct);
                return Ok(new { Message = "Product updated successfully.", Product = productDto });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productRepository.DeleteProductAsync(id);

            if (!result)
            {
                return NotFound(new { Message = $"Product with ID {id} not found." });
            }

            return Ok(new { Message = "Product deleted successfully." });
        }
    }
}
