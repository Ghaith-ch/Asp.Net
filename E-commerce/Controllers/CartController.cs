using AutoMapper;
using E_commerce.Dto.CartDtos;
using E_commerce.Models;
using E_commerce.Repositories.Interfaces;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public CartController(ICartService cartService, ICartRepository cartRepository, IMapper mapper)
        {
            _cartService = cartService;
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        // GET: api/Cart/userId/5
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<GetCartDto>> GetCartByUserId(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return NotFound(new { Message = "Cart not found." });
            }

            var cartDto = _mapper.Map<GetCartDto>(cart);
            return Ok(cartDto);
        }

        // POST: api/Cart
        [HttpPost]
        public async Task<ActionResult<GetCartDto>> CreateCart([FromBody] CreateCartDto createCartDto)
        {
            try
            {
                var cart = await _cartService.CreateCartAsync(createCartDto);
                var cartDto = _mapper.Map<GetCartDto>(cart);
                return CreatedAtAction(nameof(GetCartByUserId), new { userId = cart.UserId }, cartDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        // DELETE: api/Cart/5
        [HttpDelete("{cartId}")]
        public async Task<IActionResult> DeleteCart(int cartId)
        {
            try
            {
                await _cartService.DeleteCartAsync(cartId);
                return Ok(new { Message = "Cart deleted and stock restored successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
