using ClothStore_Backend.Application.IService;
using ClothStore_Backend.Data.Entities;
using ClothStore_Backend.Data.IRepository;

namespace ClothStore_Backend.Application.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync() => await _userRepository.GetAllAsync();
        public async Task<ApplicationUser?> GetUserByIdAsync(string id) => await _userRepository.GetByIdAsync(id);

    }
}
