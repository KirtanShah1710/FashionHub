using System.ComponentModel.DataAnnotations;

namespace ClothStore_Backend.Data.Entities
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        // Exact variant ordered
        public int ProductVariantId { get; set; }
        public ProductVariant? ProductVariant { get; set; }

        public int Quantity { get; set; }

        // Price at time of order (VERY IMPORTANT)
        public decimal Price { get; set; }
    }
}
