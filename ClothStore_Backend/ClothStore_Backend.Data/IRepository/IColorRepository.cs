using ClothStore_Backend.Data.Entities;

namespace ClothStore_Backend.Data.IRepository
{
    public interface IColorRepository
    {
        Task<Color> CreateAsync(Color color);
        Task<List<Color>> GetAllAsync();
        Task<Color?> GetByIdAsync(int id);
        Task<bool> ExistsAsync(string name);
        Task UpdateAsync(Color color);
        Task DeleteAsync(Color color);
    }
}
