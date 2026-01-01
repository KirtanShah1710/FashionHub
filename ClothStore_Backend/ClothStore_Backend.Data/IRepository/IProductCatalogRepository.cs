using ClothStore_Backend.Data.Entities;
using ClothStore_Backend.Data.Queries;

namespace ClothStore_Backend.Data.IRepository
{
    public interface IProductCatalogRepository
    {
        Task<(List<ProductVariant> Variants, int TotalCount)> GetVariantsAsync(ProductVariantQuery query);
    }
}
