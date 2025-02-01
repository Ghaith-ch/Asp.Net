using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce.Models
{
    // Represents an item in an order in the e-commerce application
    public class OrderItem
    {
        // Primary key for the OrderItem entity
        [Key]
        public int OrderItemId { get; set; }

        // The quantity of the product in the order
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        // The price of the product at the time of order
        [Required(ErrorMessage = "Price is required.")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }

        // Foreign key to associate the order item with the order
        [Required(ErrorMessage = "Order ID is required.")]
        public int OrderId { get; set; }

        // Foreign key to associate the order item with the product
        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductId { get; set; }

        // Navigation property to link the order item to the corresponding order
        [ForeignKey("OrderId")]
        public required Order Order { get; set; } // Each order item must be linked to an order

        // Navigation property to link the order item to the corresponding product
        [ForeignKey("ProductId")]
        public required Product Product { get; set; } // Each order item must reference a product
    }
}
