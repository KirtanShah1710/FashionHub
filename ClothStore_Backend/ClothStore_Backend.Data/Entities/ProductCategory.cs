using System.ComponentModel.DataAnnotations;

namespace ClothStore_Backend.Data.Entities
{
    public class ProductCategory
    {
        [Key]
        public int ProductCategoryId { get; set; }
        public string? ProCatName { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}
