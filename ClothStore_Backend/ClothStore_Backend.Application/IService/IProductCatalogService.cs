using ClothStore_Backend.Application.DTOs.RequestDto;
using ClothStore_Backend.Application.DTOs.ResponseDto;

namespace ClothStore_Backend.Application.IService
{
    public interface IProductCatalogService
    {
        Task<ApiResponse<List<ProductVariantResponseDto>>> GetCatalogAsync(ProductVariantQueryDto queryDto);
    }
}
