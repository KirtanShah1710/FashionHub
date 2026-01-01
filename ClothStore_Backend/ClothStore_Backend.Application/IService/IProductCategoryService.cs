using ClothStore_Backend.Application.DTOs.RequestDto;
using ClothStore_Backend.Application.DTOs.ResponseDto;

namespace ClothStore_Backend.Application.IService
{
    public interface IProductCategoryService
    {
        Task<ProductCategoryResponseDto> CreateAsync(ProductCategoryDto dto);
        Task<List<ProductCategoryResponseDto>> GetAllAsync();
        Task<ProductCategoryResponseDto?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(string name);
        Task<bool> UpdateAsync(int id, ProductCategoryDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
