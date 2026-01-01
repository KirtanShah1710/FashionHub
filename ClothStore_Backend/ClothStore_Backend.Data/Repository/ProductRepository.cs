using ClothStore_Backend.Data.Context;
using ClothStore_Backend.Data.Entities;
using ClothStore_Backend.Data.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ClothStore_Backend.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.GenderCategory)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.GenderCategory)
                .ToListAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Product product)
        {
            _context.ProductVariants.RemoveRange(
                _context.ProductVariants.Where(v => v.ProductId == product.ProductId));
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
