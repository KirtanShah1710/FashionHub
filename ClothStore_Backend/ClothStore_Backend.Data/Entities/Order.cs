using ClothStore_Backend.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace ClothStore_Backend.Data.Entities
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        // Logged-in user
        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending; // Pending, Shipped, Delivered

        // 🔥 NEW
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        // 🔥 Gateway references
        // ClothStore_Backend.Data.Entities.Order
        public string? RazorpayOrderId { get; set; }
        public string? RazorpayPaymentId { get; set; }


        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
