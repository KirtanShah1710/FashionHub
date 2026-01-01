using System.ComponentModel.DataAnnotations;

namespace ClothStore_Backend.Data.Entities
{
    public class Size
    {
        [Key]
        public int SizeId { get; set; }
        public string? Name { get; set; }

        public ICollection<ProductVariant>? ProductVariants { get; set; }
    }
}
