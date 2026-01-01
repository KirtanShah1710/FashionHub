using ClothStore_Backend.Data.Context;
using ClothStore_Backend.Data.Entities;
using ClothStore_Backend.Data.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ClothStore_Backend.Data.Repository
{
    public class GenderCategoryRepository : IGenderCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public GenderCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GenderCategory> CreateAsync(GenderCategory category)
        {
            _context.GenderCategories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<List<GenderCategory>> GetAllAsync()
        {
            return await _context.GenderCategories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<GenderCategory?> GetByIdAsync(int id)
        {
            return await _context.GenderCategories.FindAsync(id);
        }

        public async Task<bool> ExistsAsync(string name)
        {
            return await _context.GenderCategories
                .AnyAsync(g => g.GenCatName == name);
        }

        public async Task UpdateAsync(GenderCategory category)
        {
            _context.GenderCategories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(GenderCategory category)
        {
            _context.GenderCategories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
