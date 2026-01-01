using ClothStore_Backend.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
