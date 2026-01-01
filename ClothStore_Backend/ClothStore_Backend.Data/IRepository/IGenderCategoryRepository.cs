using ClothStore_Backend.Data.Entities;

namespace ClothStore_Backend.Data.IRepository
{
    public interface IGenderCategoryRepository
    {
        Task<GenderCategory> CreateAsync(GenderCategory category);
        Task<List<GenderCategory>> GetAllAsync();
        Task<GenderCategory?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(string name);
        Task UpdateAsync(GenderCategory category);
        Task DeleteAsync(GenderCategory category);
    }
}
