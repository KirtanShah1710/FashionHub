using System.ComponentModel.DataAnnotations;

namespace ClothStore_Backend.Data.Entities
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        public string ProductName { get; set; } = null!;
        public string? Description { get; set; }

        // Default / thumbnail image
        public string? ProductImageUrl { get; set; }

        public int ProductCategoryId { get; set; }
        public ProductCategory? ProductCategory { get; set; }

        public int GenderCategoryId { get; set; }
        public GenderCategory? GenderCategory { get; set; }

        public ICollection<ProductVariant>? Variants { get; set; }
    }
}