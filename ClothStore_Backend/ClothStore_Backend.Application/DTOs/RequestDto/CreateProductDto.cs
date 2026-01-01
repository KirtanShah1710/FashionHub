using Microsoft.AspNetCore.Http;

namespace ClothStore_Backend.Application.DTOs.RequestDto
{
    public class CreateProductDto
    {
        public string ProductName { get; set; } = null!;
        public string? Description { get; set; }

        public int ProductCategoryId { get; set; }
        public int GenderCategoryId { get; set; }

        public IFormFile? Image { get; set; }
    }
}
