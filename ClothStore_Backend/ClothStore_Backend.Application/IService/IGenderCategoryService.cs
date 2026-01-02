using ClothStore_Backend.Application.DTOs.RequestDto;
using ClothStore_Backend.Application.DTOs.ResponseDto;

namespace ClothStore_Backend.Application.IService
{
    public interface IGenderCategoryService
    {
        Task<GenderCategoryResponseDto> CreateAsync(GenderCategoryDto dto);
        Task<List<GenderCategoryResponseDto>> GetAllAsync();
        Task<GenderCategoryResponseDto?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(string name);
        Task<bool> UpdateAsync(int id, GenderCategoryDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
