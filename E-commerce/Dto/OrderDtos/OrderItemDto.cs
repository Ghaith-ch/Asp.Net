using System;
using System.ComponentModel.DataAnnotations;

namespace E_commerce.Dto.OrderDtos
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; } 
    }
}