using System;
using System.ComponentModel.DataAnnotations;

namespace E_commerce.Dto.UserDtos;

public class RegisterUserDto
{
        // The desired username of the user, required for registration
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(100, ErrorMessage = "Username cannot exceed 100 characters.")]
    public required string Username { get; set; }

    // The email address of the user, required for registration
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters.")]
    public required string Email { get; set; }

    // The password of the user, required for registration, must be at least 6 characters
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(255, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
    public required string Password { get; set; }

    // The role assigned to the user (e.g., "Admin", "Customer"). Default is "Customer".
    public string? Role { get; set; }

    // The user's address (optional)
    public string? Address { get; set; }

    // The user's phone number (optional)
    public string? PhoneNumber { get; set; }
}

