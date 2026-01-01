using System.ComponentModel.DataAnnotations;

namespace ClothStore_Backend.Data.Entities
{
    public class ProductVariant
    {
        [Key]
        public int ProductVariantId { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int SizeId { get; set; }
        public Size? Size { get; set; }

        public int ColorId { get; set; }
        public Color? Color { get; set; }

        public decimal Price { get; set; }
        public int Stock { get; set; }

        // Color-wise image
        public string? ImageUrl { get; set; }

        public string? SKU { get; set; }
    }
}
