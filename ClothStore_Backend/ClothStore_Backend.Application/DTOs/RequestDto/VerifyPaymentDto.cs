namespace ClothStore_Backend.Application.DTOs.RequestDto
{
    public class VerifyPaymentDto
    {
        public int OrderId { get; set; }
        public string RazorpayPaymentId { get; set; } = null!;
        public string RazorpayOrderId { get; set; } = null!;
        public string RazorpaySignature { get; set; } = null!;
    }
}