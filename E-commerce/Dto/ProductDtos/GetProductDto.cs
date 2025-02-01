using E_commerce.Dto.CategoryDtos;
namespace E_commerce.Dto.ProductDtos;
public class GetProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;  
        public string? Description { get; set; }  
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? ImageUrl { get; set; } 
        public GetCategoryDto? Category { get; set; }  
    }
