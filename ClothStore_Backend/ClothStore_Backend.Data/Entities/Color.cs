using System.ComponentModel.DataAnnotations;

namespace ClothStore_Backend.Data.Entities
{
    public class Color
    {
        [Key]
        public int ColorId { get; set; }
        public string? ColorName { get; set; }
        public string? HexCode { get; set; }

        public ICollection<ProductVariant>? ProductVariants { get; set; }
    }
}
