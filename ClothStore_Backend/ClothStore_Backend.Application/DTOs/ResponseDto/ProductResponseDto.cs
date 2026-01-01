namespace ClothStore_Backend.Application.DTOs.ResponseDto
{
    public class ProductResponseDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? Description { get; set; }
        public string? ProductImageUrl { get; set; }

        public string ProductCategory { get; set; } = null!;
        public string GenderCategory { get; set; } = null!;
    }
}
