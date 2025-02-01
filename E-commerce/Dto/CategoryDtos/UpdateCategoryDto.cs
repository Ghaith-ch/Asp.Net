using System.ComponentModel.DataAnnotations;

namespace E_commerce.Dto.CategoryDtos
{
    public class UpdateCategoryDto
    {
        // Name of the category, which cannot exceed 100 characters
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        public string? Name { get; set; }

        // Optional description of the category, cannot exceed 500 characters
        [StringLength(500, ErrorMessage = "Category description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        // Foreign key to the parent category, nullable for top-level categories
        public int? ParentCategoryId { get; set; } 
    }
    
}
