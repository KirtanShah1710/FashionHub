using ClothStore_Backend.Application.DTOs.RequestDto;
using ClothStore_Backend.Application.DTOs.ResponseDto;
using ClothStore_Backend.Application.IService;
using ClothStore_Backend.Data.Entities;
using ClothStore_Backend.Data.IRepository;
using ClothStore_Backend.Data.Queries;

namespace ClothStore_Backend.Application.Service
{
    public class ProductCatalogService : IProductCatalogService
    {
        private readonly IProductCatalogRepository _catalogRepository;
        
        public ProductCatalogService(IProductCatalogRepository catalogRepository)
        {
            _catalogRepository = catalogRepository;
        }

        public async Task<ApiResponse<List<ProductVariantResponseDto>>> 
            GetCatalogAsync(ProductVariantQueryDto dto)
        {
            var query = new ProductVariantQuery
            {
                Search = dto.Search,
                ProductCategoryId = dto.ProductCategoryId,
                GenderCategoryId = dto.GenderCategoryId,
                ColorId = dto.ColorId,
                SizeId = dto.SizeId,
                MinPrice = dto.MinPrice,
                MaxPrice = dto.MaxPrice,
                SortBy = dto.SortBy,
                SortOrder = dto.SortOrder,
                PageNumber = dto.PageNumber,
                PageSize = dto.PageSize
            };

            // 1️ Always fetch VARIANTS
            var (variants, _) = await _catalogRepository.GetVariantsAsync(query);

            // 2️ Group variants by Product
            var variantsByProduct = variants
                .GroupBy(v => v.ProductId)
                .ToDictionary(g => g.Key, g => g.ToList());

            // 3️ FINAL GROUPING COLOR
            var groupedVariants = variants
                .GroupBy(v => new { v.ProductId, v.ColorId })
                .Select(g => g.First())
                .ToList();

            int totalCount = groupedVariants.Count;

            // 4️ Map response
            var response = groupedVariants.Select(v =>
            {
                var productVariants = variantsByProduct[v.ProductId];

                return new ProductVariantResponseDto
                {
                    ProductVariantId = v.ProductVariantId,

                    ProductId = v.Product!.ProductId,
                    ProductName = v.Product.ProductName!,

                    ColorId = v.ColorId,
                    Color = v.Color!.ColorName!,
                    HexCode = v.Color.HexCode,

                    SizeId = v.SizeId,
                    Size = v.Size!.Name!,

                    Price = v.Price,
                    Stock = v.Stock,
                    ImageUrl = v.ImageUrl,

                    AvailableColors = BuildColorOptions(productVariants),
                    AvailableSizes = BuildSizeOptions(productVariants)
                };
            }).ToList();

            return new ApiResponse<List<ProductVariantResponseDto>>
            {
                Success = true,
                Message = "Products fetched successfully",
                Data = response,
                Meta = new
                {
                    totalCount,
                    pageNumber = dto.PageNumber,
                    pageSize = dto.PageSize,
                    totalPages = (int)Math.Ceiling(
                        (double)totalCount / dto.PageSize)
                }
            };
        }

        // helper function to get available colors and sizes
        private List<ColorResponseDto> BuildColorOptions(List<ProductVariant> productVariants)
        {
            return productVariants
                .GroupBy(v => v.ColorId)
                .Select(g =>
                {
                    var first = g.First();
                    return new ColorResponseDto
                    {
                        ColorId = g.Key,
                        Name = first.Color!.ColorName!,
                        // note that we are using ImageUrl as HexCode to show color swatch
                        HexCode = first.ImageUrl!
                    };
                })
                .ToList();
        }

        private List<SizeResponseDto> BuildSizeOptions(List<ProductVariant> productVariants)
        {

            return productVariants
                .GroupBy(v => v.SizeId)
                .Select(g =>
                {
                    var first = g.First();
                    return new SizeResponseDto
                    {
                        SizeId = g.Key,
                        Name = first.Size!.Name!
                    };
                })
                .ToList();
        }
    }
}