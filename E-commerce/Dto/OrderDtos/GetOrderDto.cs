namespace E_commerce.Dto.OrderDtos
{
    public class GetOrderDto
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public List<OrderItemDto>? OrderItems { get; set; }
        public decimal TotalAmount { get; set; } 
        public DateTime OrderDate { get; set; }
        public string? Status { get; set; }
    }
}
