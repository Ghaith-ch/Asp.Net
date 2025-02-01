using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce.Models
{
    // Represents a product category in the e-commerce application
    public class Category
    {
        // Primary key for the Category entity
        [Key]
        public int CategoryId { get; set; }

        // Name of the category, which must be unique and is required
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        public required string Name { get; set; } 

        // Optional description of the category
        [StringLength(500, ErrorMessage = "Category description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        // Foreign key to the parent category, nullable for top-level categories
        public int? ParentCategoryId { get; set; }

        // Navigation property to link the category to its parent category
        [ForeignKey("ParentCategoryId")]
        public Category? ParentCategory { get; set; } // Nullable since some categories may be top-level

        // Collection of subcategories for this category
        public ICollection<Category>? Subcategories { get; set; } = new List<Category>();

        // Collection of products associated with this category
        public ICollection<Product>? Products { get; set; } = new List<Product>();
    }
}
