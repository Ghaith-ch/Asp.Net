using System.ComponentModel.DataAnnotations;

namespace E_commerce.Dto.ProductDtos
{
    public class UpdateProductDto
    {
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string? Name { get; set; } 

        [StringLength(500, ErrorMessage = "Product description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal? Price { get; set; } 

        [Range(0, int.MaxValue, ErrorMessage = "Stock must be zero or more.")]
        public int? Stock { get; set; } 

        [StringLength(2083, ErrorMessage = "Image URL cannot exceed 2083 characters.")]
        public string? ImageUrl { get; set; } 

        public int? CategoryId { get; set; } 
    }
}
