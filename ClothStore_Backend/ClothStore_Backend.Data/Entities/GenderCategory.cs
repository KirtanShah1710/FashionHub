using System.ComponentModel.DataAnnotations;

namespace ClothStore_Backend.Data.Entities
{
    public class GenderCategory
    {
        [Key]
        public int GenderCategoryId { get; set; }
        public string? GenCatName { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}
