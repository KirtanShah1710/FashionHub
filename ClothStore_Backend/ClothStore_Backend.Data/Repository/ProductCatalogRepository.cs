using ClothStore_Backend.Data.Context;
using ClothStore_Backend.Data.Entities;
using ClothStore_Backend.Data.IRepository;
using ClothStore_Backend.Data.Queries;
using Microsoft.EntityFrameworkCore;


namespace ClothStore_Backend.Data.Repository
{
    public class ProductCatalogRepository : IProductCatalogRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductCatalogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(List<ProductVariant>, int)> GetVariantsAsync(ProductVariantQuery query)
        {
            // root query
            IQueryable<ProductVariant> q = _context.ProductVariants
                .Include(v => v.Product)
                    .ThenInclude(p => p!.ProductCategory)
                .Include(v => v.Product)
                    .ThenInclude(p => p!.GenderCategory)
                .Include(v => v.Color)
                .Include(v => v.Size);

            // search
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                q = q.Where(v =>
                    v.Product!.ProductName!.Contains(query.Search) ||
                    v.Product!.Description!.Contains(query.Search)
                    );
            }

            // product level filters
            if (query.ProductCategoryId?.Any() == true)
            {
                q = q.Where(v =>
                    query.ProductCategoryId.Contains(v.Product!.ProductCategoryId));
            }

            if (query.GenderCategoryId?.Any() == true)
            {
                q = q.Where(v =>
                    query.GenderCategoryId.Contains(v.Product!.GenderCategoryId));
            }

            // variant level filters
            if (query.ColorId?.Any() == true)
            {
                q = q.Where(v =>
                    query.ColorId.Contains(v.ColorId));
            }

            if (query.SizeId?.Any() == true)
            {
                q = q.Where(v =>
                    query.SizeId.Contains(v.SizeId));
            }

            if (query.MinPrice.HasValue)
            {
                q = q.Where(v => v.Price >= query.MinPrice);
            }

            if (query.MaxPrice.HasValue)
            {
                q = q.Where(v => v.Price <= query.MaxPrice);
            }

            // total count before pagination
            int totalCount = await q.CountAsync();

            // sorting (price only for now)
            q = query.SortOrder.ToLower() == "desc"
                ? q.OrderByDescending(v => v.Price)
                : q.OrderBy(v => v.Price);

            // pagination applied to varient
            q = q
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize);

            return (await q.ToListAsync(), totalCount);
        }
    }
}
