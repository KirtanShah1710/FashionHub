namespace ClothStore_Backend.Application.DTOs.ResponseDto
{
    public class ColorResponseDto
    {
        public int ColorId { get; set; }
        public string Name { get; set; } = null!;
        public string? HexCode { get; set; }
    }
}
