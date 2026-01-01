namespace ClothStore_Backend.Application.DTOs.ResponseDto
{
    public class ProductVariantResponseDto
    {
        public int ProductVariantId { get; set; }

        // Product
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;

        // Size
        public int SizeId { get; set; }
        public string Size { get; set; } = null!;

        // Color
        public int ColorId { get; set; }
        public string Color { get; set; } = null!;
        public string? HexCode { get; set; }

        // Variant details
        public decimal Price { get; set; }
        public int Stock { get; set; }

        // Color-wise image
        public string? ImageUrl { get; set; }

        // 
        public List<ColorResponseDto> AvailableColors { get; set; } = [];
        public List<SizeResponseDto> AvailableSizes { get; set; } = [];
    }
}
