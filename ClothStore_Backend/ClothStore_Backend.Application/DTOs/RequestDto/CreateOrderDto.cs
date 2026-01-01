namespace ClothStore_Backend.Application.DTOs.RequestDto
{
    public class CreateOrderDto
    {
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
    }
}
