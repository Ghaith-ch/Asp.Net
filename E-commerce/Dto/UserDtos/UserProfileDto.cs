using System;
using E_commerce.Dto.OrderDtos;

namespace E_commerce.Dto.UserDtos;

public class UserProfileDto
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; } 

    public List<GetOrderDto>? Orders { get; set; } 
}


