using ClothStore_Backend.Data.Entities;

namespace ClothStore_Backend.Data.IRepository
{
    public interface IProductCategoryRepository
    {
        Task<ProductCategory> CreateAsync(ProductCategory category);
        Task<List<ProductCategory>> GetAllAsync();
        Task<ProductCategory?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(string name);
        Task UpdateAsync(ProductCategory category);
        Task DeleteAsync(ProductCategory category);
    }
}
