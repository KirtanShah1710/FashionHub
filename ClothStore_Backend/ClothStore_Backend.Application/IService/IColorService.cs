using ClothStore_Backend.Application.DTOs.RequestDto;
using ClothStore_Backend.Application.DTOs.ResponseDto;

namespace ClothStore_Backend.Application.IService
{
    public interface IColorService
    {
        Task<ColorResponseDto> CreateAsync(ColorDto dto);
        Task<List<ColorResponseDto>> GetAllAsync();
        Task<ColorResponseDto?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(string name);
        Task<bool> UpdateAsync(int id, ColorDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
