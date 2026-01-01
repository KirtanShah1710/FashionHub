using Microsoft.AspNetCore.Http;

namespace ClothStore_Backend.Application.DTOs.RequestDto
{
    public class CreateProductVariantDto
    {
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int ColorId { get; set; }

        public decimal Price { get; set; }
        public int Stock { get; set; }

        public IFormFile? Image { get; set; }

    }
}
