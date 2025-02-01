using System;
using System.ComponentModel.DataAnnotations;

namespace E_commerce.Dto.UserDtos;

public class LoginUserDto
{
    // The email address of the user, required for login
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public required string Email { get; set; }

    // The password of the user, required for login
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(255, ErrorMessage = "Password cannot exceed 255 characters.")]
    public required string Password { get; set; }
}

