namespace ClothStore_Backend.Application.DTOs.RequestDto
{
    public class CreatePaymentOrderDto
    {
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
    }
}