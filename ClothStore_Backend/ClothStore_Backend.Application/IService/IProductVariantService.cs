using ClothStore_Backend.Application.DTOs.RequestDto;
using ClothStore_Backend.Application.DTOs.ResponseDto;

namespace ClothStore_Backend.Application.IService
{
    public interface IProductVariantService
    {
        Task<List<ProductVariantResponseDto>> GetAllAsync();
        Task<ProductVariantResponseDto?> GetByIdAsync(int id);
        Task CreateAsync(CreateProductVariantDto dto);
        Task<bool> UpdateAsync(int id, CreateProductVariantDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<ProductVariantResponseDto>> GetByProductIdAsync(int productId);

    }
}
