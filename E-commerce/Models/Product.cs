using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce.Models
{
    // Represents a product in the e-commerce application
    public class Product
    {
        // Primary key for the Product entity
        [Key]
        public int ProductId { get; set; }

        // Name of the product, which must be unique and is required
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        // Optional description of the product
        [StringLength(500, ErrorMessage = "Product description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        // Price of the product, which must be greater than zero
        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        [Column(TypeName = "decimal(18,2)")] // Specifies precision and scale for monetary values
        public decimal Price { get; set; }

        // Current stock level of the product, which must be zero or greater
        [Required(ErrorMessage = "Stock quantity is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be zero or more.")]
        public int Stock { get; set; }

        // URL for the product image
        [StringLength(2083, ErrorMessage = "Image URL cannot exceed 2083 characters.")]
        public string? ImageUrl { get; set; } // Nullable since not all products may have an image

        // Foreign key to associate the product with a category
        [Required(ErrorMessage = "Category ID is required.")]
        public int CategoryId { get; set; }

        // Navigation property to link the product to its category
        [ForeignKey("CategoryId")]
        public required Category Category { get; set; } // Required since every product must belong to a category
    }
}
