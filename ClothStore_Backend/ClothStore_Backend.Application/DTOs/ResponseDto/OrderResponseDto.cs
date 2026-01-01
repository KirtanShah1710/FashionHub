namespace ClothStore_Backend.Application.DTOs.ResponseDto
{
    public class OrderResponseDto
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public string? RazorpayOrderId { get; set; }
        public string Status { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public string PaymentStatus { get; set; } = null!;
    }
}
