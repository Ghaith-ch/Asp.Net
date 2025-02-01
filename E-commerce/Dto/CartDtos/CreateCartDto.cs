using System.ComponentModel.DataAnnotations;

namespace E_commerce.Dto.CartDtos
{
    public class CreateCartDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Cart must have at least one item.")]
        public List<CreateCartItemDto> CartItems { get; set; } = new List<CreateCartItemDto>();
    }
}
