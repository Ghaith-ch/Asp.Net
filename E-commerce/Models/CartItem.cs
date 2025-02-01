using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce.Models
{
    // Represents an item in a shopping cart
    public class CartItem
    {
        // Primary key for the CartItem entity
        [Key]
        public int CartItemId { get; set; }

        // Foreign key to associate the cart item with the cart
        [Required(ErrorMessage = "Cart ID is required.")]
        public int CartId { get; set; }

        // Navigation property to link the cart item to its parent cart
        [ForeignKey("CartId")]
        public required Cart Cart { get; set; } // Required since each cart item must belong to a cart

        // Foreign key to associate the cart item with a product
        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductId { get; set; }

        // Navigation property to link the cart item to the corresponding product
        [ForeignKey("ProductId")]
        public required Product Product { get; set; } // Required since each cart item must reference a product

        // The quantity of the product in the cart
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        // The price of a single unit of the product when added to the cart
        [Required(ErrorMessage = "Unit price is required.")]
        [Column(TypeName = "decimal(18,2)")] // Specify the precision for the unit price
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than zero.")]
        public decimal UnitPrice { get; set; }
    }
}
