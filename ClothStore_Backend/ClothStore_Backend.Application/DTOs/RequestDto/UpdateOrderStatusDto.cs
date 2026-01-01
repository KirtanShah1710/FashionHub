using ClothStore_Backend.Data.Enums;

namespace ClothStore_Backend.Application.DTOs.RequestDto
{
    public class UpdateOrderStatusDto
    {
        public OrderStatus Status { get; set; }
    }
}
