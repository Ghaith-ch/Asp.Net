namespace E_commerce.Dto.CartDtos
{
    public class GetCartDto
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public List<GetCartItemDto> CartItems { get; set; } = new List<GetCartItemDto>();
    }
}