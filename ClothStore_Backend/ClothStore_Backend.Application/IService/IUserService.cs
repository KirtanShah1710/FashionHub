using ClothStore_Backend.Data.Entities;

namespace ClothStore_Backend.Application.IService
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser?> GetUserByIdAsync(string id);

    }
}
