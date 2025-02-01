using System.ComponentModel.DataAnnotations;

namespace E_commerce.Dto.CategoryDtos
{
    public class CreateCategoryDto
    {
        // Name of the category, which must be unique and is required
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        // Optional description of the category
        [StringLength(500, ErrorMessage = "Category description cannot exceed 500 characters.")]
        public string? Description { get; set; } 

        // Foreign key to the parent category, nullable for top-level categories
        public int? ParentCategoryId { get; set; }
    }
    
}
