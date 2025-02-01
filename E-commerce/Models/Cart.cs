using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce.Models
{
    public class Cart
    {
        // Primary key for the Cart entity
        [Key]
        public int CartId { get; set; }

        // Foreign key to associate the cart with a specific user
        [Required(ErrorMessage = "User ID is required.")]
        public int UserId { get; set; }

        // Navigation property to link the cart to its corresponding user
        [ForeignKey("UserId")]
        public required ApplicationUser User { get; set; } 

        // Collection of cart items representing products in the user's cart
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
