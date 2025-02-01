using AutoMapper;
using E_commerce.Dto.CartDtos;
using E_commerce.Models;
using E_commerce.Repositories.Interfaces;
using E_commerce.Services.Interfaces;

namespace E_commerce.Services.Implementations;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    public CartService(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    public async Task<Cart> CreateCartAsync(CreateCartDto createCartDto)
    {
        if (createCartDto == null)
        {
            throw new ArgumentException("Invalid cart data.");
        }

        // Check if the user already has a cart
        var existingCart = await _cartRepository.GetCartByUserIdAsync(createCartDto.UserId);
        if (existingCart != null)
        {
            throw new InvalidOperationException("User already has a cart.");
        }

        var user = await _cartRepository.GetUserByIdAsync(createCartDto.UserId);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {createCartDto.UserId} does not exist.");
        }

        var cartItems = new List<CartItem>();

        // Validate each CartItem and fetch its UnitPrice
        foreach (var item in createCartDto.CartItems)
        {
            var product = await _cartRepository.GetProductByIdAsync(item.ProductId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {item.ProductId} does not exist.");
            }

            if (product.Stock < item.Quantity)
            {
                throw new InvalidOperationException($"Not enough stock for Product ID {item.ProductId}. Available: {product.Stock}");
            }

            // Reduce stock and add to cart items
            product.Stock -= item.Quantity;
            cartItems.Add(new CartItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = product.Price,
                Product = product,
                Cart = null 
            });
        }

        var cart = new Cart
        {
            UserId = createCartDto.UserId,
            User = user, 
            CartItems = cartItems
        };

        foreach (var item in cartItems)
        {
            item.Cart = cart;
        }

        return await _cartRepository.AddCartAsync(cart);
    }



    public async Task<bool> DeleteCartAsync(int cartId)
    {
        var cart = await _cartRepository.GetCartByIdAsync(cartId);
        if (cart == null)
        {
            throw new KeyNotFoundException("Cart not found.");
        }

        // Restore the stock for each cart item before deleting the cart
        foreach (var item in cart.CartItems)
        {
            var product = await _cartRepository.GetProductByIdAsync(item.ProductId);
            if (product != null)
            {
                product.Stock += item.Quantity; // Revert the stock back
            }
        }

        return await _cartRepository.RemoveCartAsync(cartId);
    }
}
