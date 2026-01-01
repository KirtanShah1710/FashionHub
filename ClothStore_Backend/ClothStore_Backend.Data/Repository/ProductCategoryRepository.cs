using ClothStore_Backend.Data.Context;
using ClothStore_Backend.Data.Entities;
using ClothStore_Backend.Data.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ClothStore_Backend.Data.Repository
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductCategory> CreateAsync(ProductCategory category)
        {
            _context.ProductCategories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<List<ProductCategory>> GetAllAsync()
        {
            return await _context.ProductCategories
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ProductCategory?> GetByIdAsync(int id)
        {
            return await _context.ProductCategories.FindAsync(id);
        }

        public async Task<bool> ExistsAsync(string name)
        {
            return await _context.ProductCategories
                .AnyAsync(c => c.ProCatName == name);
        }

        public async Task UpdateAsync(ProductCategory category)
        {
            _context.ProductCategories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ProductCategory category)
        {
            _context.ProductCategories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
