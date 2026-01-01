using ClothStore_Backend.Application.DTOs.RequestDto;
using ClothStore_Backend.Application.DTOs.ResponseDto;

namespace ClothStore_Backend.Application.IService
{
    public interface IProductService
    {
        Task<ProductResponseDto> CreateAsync(CreateProductDto dto);
        Task<List<ProductResponseDto>> GetAllAsync();
        Task<ProductResponseDto?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, CreateProductDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
