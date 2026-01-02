using ClothStore_Backend.Data.Context;
using ClothStore_Backend.Data.Entities;
using ClothStore_Backend.Data.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ClothStore_Backend.Data.Repository
{
    public class SizeRepository : ISizeRepository
    {
        private readonly ApplicationDbContext _context;

        public SizeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Size> CreateAsync(Size size)
        {
            _context.Sizes.Add(size);
            await _context.SaveChangesAsync();
            return size;
        }

        public async Task<List<Size>> GetAllAsync()
        {
            return await _context.Sizes.AsNoTracking().ToListAsync();
        }

        public async Task<Size?> GetByIdAsync(int id)
        {
            return await _context.Sizes.FindAsync(id);
        }

        public async Task<bool> ExistsAsync(string name)
        {
            return await _context.Sizes.AnyAsync(s => s.Name == name);
        }

        public async Task UpdateAsync(Size size)
        {
            _context.Sizes.Update(size);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Size size)
        {
            _context.Sizes.Remove(size);
            await _context.SaveChangesAsync();
        }
    }
}
