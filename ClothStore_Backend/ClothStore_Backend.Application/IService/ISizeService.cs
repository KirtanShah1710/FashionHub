using ClothStore_Backend.Application.DTOs.RequestDto;
using ClothStore_Backend.Application.DTOs.ResponseDto;

namespace ClothStore_Backend.Application.IService
{
    public interface ISizeService
    {
        Task<SizeResponseDto> CreateAsync(SizeDto dto);
        Task<List<SizeResponseDto>> GetAllAsync();
        Task<SizeResponseDto?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(string name);
        Task<bool> UpdateAsync(int id, SizeDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
