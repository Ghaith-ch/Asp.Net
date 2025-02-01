using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace E_commerce.Models
{
    // Represents a user in the application with additional profile fields
    public class ApplicationUser : IdentityUser<int> // Inherits from IdentityUser with integer key type
    {
        // The cart associated with this user (One-to-One)
        public Cart? Cart { get; set; } // Nullable to handle cases where a user may not have a cart

        // Collection of orders made by the user (One-to-Many)
        public ICollection<Order> Orders { get; set; } = new List<Order>(); // Initialized to avoid null reference exceptions

        // Optional address field for user profile
        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters.")] // Add a length constraint
        public string? Address { get; set; } // Nullable since the address may not be mandatory
    }
}
