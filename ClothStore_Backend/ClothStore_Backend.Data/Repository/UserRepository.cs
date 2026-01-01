using ClothStore_Backend.Data.Context;
using ClothStore_Backend.Data.Entities;
using ClothStore_Backend.Data.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClothStore_Backend.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<ApplicationUser?> GetByEmailAsync(string email) => await _userManager.FindByEmailAsync(email);
        public async Task<ApplicationUser?> GetByIdAsync(string id) => await _userManager.FindByIdAsync(id);
        public async Task<IEnumerable<ApplicationUser>> GetAllAsync() => await _context.Users.ToListAsync();

        public async Task<bool> CreateUserAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return false;

            // Assign default role
            await _userManager.AddToRoleAsync(user, "Customer");

            return true;
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password) =>
            await _userManager.CheckPasswordAsync(user, password);

    }
}
