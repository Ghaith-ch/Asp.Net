using System;
using E_commerce.Dto.CartDtos;
using E_commerce.Models;

namespace E_commerce.Services.Interfaces;

public interface ICartService
{
    Task<Cart> CreateCartAsync(CreateCartDto createCartDto);
    Task<bool> DeleteCartAsync(int cartId);
}