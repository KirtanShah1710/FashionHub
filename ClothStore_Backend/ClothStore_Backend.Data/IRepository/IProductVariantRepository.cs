using ClothStore_Backend.Data.Entities;

namespace ClothStore_Backend.Data.IRepository
{
    public interface IProductVariantRepository
    {
        Task<List<ProductVariant>> GetAllAsync();
        Task<ProductVariant?> GetByIdAsync(int id);
        Task CreateAsync(ProductVariant variant);
        Task UpdateAsync(ProductVariant variant);
        Task DeleteAsync(ProductVariant variant);
        Task<List<ProductVariant>> GetByProductIdAsync(int productId);
    }
}
