using ClothStore_Backend.Data.Context;
using ClothStore_Backend.Data.Entities;
using ClothStore_Backend.Data.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ClothStore_Backend.Data.Repository
{
    public class ColorRepository : IColorRepository
    {
        private readonly ApplicationDbContext _context;

        public ColorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Color> CreateAsync(Color color)
        {
            _context.Colors.Add(color);
            await _context.SaveChangesAsync();
            return color;
        }

        public async Task<List<Color>> GetAllAsync()
        {
            return await _context.Colors
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Color?> GetByIdAsync(int id)
        {
            return await _context.Colors.FindAsync(id);
        }

        public async Task<bool> ExistsAsync(string name)
        {
            return await _context.Colors
                .AnyAsync(c => c.ColorName == name);
        }

        public async Task UpdateAsync(Color color)
        {
            _context.Colors.Update(color);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Color color)
        {
            _context.Colors.Remove(color);
            await _context.SaveChangesAsync();
        }
    }
}
