using ClothStore_Backend.Data.Context;
using ClothStore_Backend.Data.Entities;
using ClothStore_Backend.Data.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ClothStore_Backend.Data.Repository
{
    public class ProductVariantRepository : IProductVariantRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductVariantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductVariant>> GetAllAsync()
        {
            return await _context.ProductVariants
                .Include(v => v.Product)
                .Include(v => v.Size)
                .Include(v => v.Color)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ProductVariant?> GetByIdAsync(int id)
        {
            return await _context.ProductVariants
                .Include(v => v.Product)
                .Include(v => v.Size)
                .Include(v => v.Color)
                .FirstOrDefaultAsync(v => v.ProductVariantId == id);
        }

        public async Task CreateAsync(ProductVariant variant)
        {
            _context.ProductVariants.Add(variant);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProductVariant variant)
        {
            _context.ProductVariants.Update(variant);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ProductVariant variant)
        {
            _context.ProductVariants.Remove(variant);
            await _context.SaveChangesAsync();
        }
        public async Task<List<ProductVariant>> GetByProductIdAsync(int productId)
        {
            return await _context.ProductVariants
                .Include(v => v.Size)
                .Include(v => v.Color)
                .Include(v => v.Product)
                .Where(v => v.ProductId == productId)
                .ToListAsync();
        }
    }
}
