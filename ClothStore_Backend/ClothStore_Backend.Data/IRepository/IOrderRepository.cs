using ClothStore_Backend.Data.Entities;

namespace ClothStore_Backend.Data.IRepository
{
    public interface IOrderRepository
    {
        Task CreateAsync(Order order);
        Task<Order?> GetByIdAsync(int orderId);
        Task<List<Order>> GetByUserIdAsync(int userId);
        Task<List<Order>> GetAllAsync();
        Task UpdateAsync(Order order);
    }
}
