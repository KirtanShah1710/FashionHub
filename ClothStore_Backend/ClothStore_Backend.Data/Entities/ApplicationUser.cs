using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ClothStore_Backend.Data.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string? Name { get; set; }
        public int? Age { get; set; }
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public override string? Email { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}
