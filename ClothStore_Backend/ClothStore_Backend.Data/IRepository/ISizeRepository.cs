using ClothStore_Backend.Data.Entities;

namespace ClothStore_Backend.Data.IRepository
{
    public interface ISizeRepository
    {
        Task<Size> CreateAsync(Size size);
        Task<List<Size>> GetAllAsync();
        Task<Size?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(string name);
        Task UpdateAsync(Size size);
        Task DeleteAsync(Size size);
    }
}
