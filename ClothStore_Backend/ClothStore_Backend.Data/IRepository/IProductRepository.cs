using ClothStore_Backend.Data.Entities;

namespace ClothStore_Backend.Data.IRepository
{
    public interface IProductRepository
    {
        Task<Product> CreateAsync(Product product);
        Task<Product?> GetByIdAsync(int id);
        Task<List<Product>> GetAllAsync();
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
    }
}
