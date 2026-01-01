//using ClothStore_Backend.Application.DTOs.RequestDto;
//using ClothStore_Backend.Application.DTOs.ResponseDto;
//using ClothStore_Backend.Application.IService;
//using ClothStore_Backend.Data.Entities;
//using ClothStore_Backend.Data.IRepository;
//using ClothStore_Backend.Data.Queries;

//namespace ClothStore_Backend.Application.Service
//{
//    //public class ProductCatalogService : IProductCatalogService
//    //{
//    //    private readonly IProductCatalogRepository _catalogRepository;
//    //    private readonly IProductVariantRepository _variantRepository;

//    //    public ProductCatalogService(
//    //        IProductCatalogRepository catalogRepository,
//    //        IProductVariantRepository variantRepository)
//    //    {
//    //        _catalogRepository = catalogRepository;
//    //        _variantRepository = variantRepository;
//    //    }

//    //    public async Task<ApiResponse<List<ProductVariantResponseDto>>> GetCatalogAsync(ProductVariantQueryDto dto)
//    //    {
//    //        // map application query dto to data query
//    //        var query = new ProductVariantQuery
//    //        {
//    //            Search = dto.Search,
//    //            ProductCategoryId = dto.ProductCategoryId,
//    //            GenderCategoryId = dto.GenderCategoryId,
//    //            ColorId = dto.ColorId,
//    //            SizeId = dto.SizeId,
//    //            MinPrice = dto.MinPrice,
//    //            MaxPrice = dto.MaxPrice,
//    //            SortBy = dto.SortBy,
//    //            SortOrder = dto.SortOrder,
//    //            PageNumber = dto.PageNumber,
//    //            PageSize = dto.PageSize
//    //        };

//    //        // Call repository (ALWAYS variant-level)
//    //        var (variants, variantTotalCount) =
//    //            await _catalogRepository.GetVariantsAsync(query);

//    //        // groping varient by product
//    //        var variantsByProduct = variants
//    //            .GroupBy(v => v.ProductId)
//    //            .ToDictionary(g => g.Key, g => g.ToList());

//    //        // Detect filter/search mode
//    //        bool isFilterMode =
//    //            !string.IsNullOrWhiteSpace(dto.Search) ||
//    //            dto.SizeId?.Any() == true ||
//    //            dto.ColorId?.Any() == true ||
//    //            dto.ProductCategoryId?.Any() == true ||
//    //            dto.GenderCategoryId?.Any() == true ||
//    //            dto.MinPrice.HasValue ||
//    //            dto.MaxPrice.HasValue;

//    //        List<ProductVariant> finalVariants;
//    //        int totalCount;

//    //        if (!isFilterMode)
//    //        {
//    //            // DEFAULT MODE → one variant per product
//    //            finalVariants = variants
//    //                .GroupBy(v => v.ProductId)
//    //                .Select(g => g.First())
//    //                .ToList();

//    //            totalCount = finalVariants.Count; // product count
//    //        }
//    //        else
//    //        {
//    //            // FILTER MODE → all variants
//    //            finalVariants = variants;
//    //            totalCount = variantTotalCount; // variant count
//    //        }

//    //        // Map response DTO
//    //        var response = finalVariants.Select(v =>
//    //        {
//    //            var productVariants = variantsByProduct[v.ProductId];

//    //            return new ProductVariantResponseDto
//    //            {
//    //                ProductVariantId = v.ProductVariantId,

//    //                ProductId = v.Product?.ProductId ?? 0,
//    //                ProductName = v.Product?.ProductName ?? "",

//    //                SizeId = v.SizeId,
//    //                Size = v.Size?.Name ?? "",

//    //                ColorId = v.ColorId,
//    //                Color = v.Color?.ColorName ?? "",
//    //                HexCode = v.Color?.HexCode,

//    //                Price = v.Price,
//    //                Stock = v.Stock,
//    //                ImageUrl = v.ImageUrl,

//    //                // DISTINCT OPTIONS
//    //                AvailableColors = BuildColorOptions(productVariants),
//    //                AvailableSizes = BuildSizeOptions(productVariants)
//    //            };
//    //        }).ToList();


//    //        // Centralized response
//    //        return new ApiResponse<List<ProductVariantResponseDto>>
//    //        {
//    //            Success = true,
//    //            Message = "Products fetched successfully",
//    //            Data = response,
//    //            Meta = new
//    //            {
//    //                totalCount,
//    //                pageNumber = dto.PageNumber,
//    //                pageSize = dto.PageSize,
//    //                totalPages = (int)Math.Ceiling(
//    //                    (double)totalCount / dto.PageSize)
//    //            }
//    //        };
//    //    }

//    //        // helper function to get available colors and sizes
//    //        private List<ColorResponseDto> BuildColorOptions(List<ProductVariant> productVariants)
//    //        {
//    //            return productVariants
//    //                .GroupBy(v => v.ColorId)
//    //                .Select(g =>
//    //                {
//    //                    var first = g.First();
//    //                    return new ColorResponseDto
//    //                    {
//    //                        ColorId = g.Key,
//    //                        Name = first.Color!.ColorName!,
//    //                        //HexCode = first.Color.HexCode
//    //                        // note that we are using ImageUrl as HexCode to show color swatch
//    //                        HexCode = first.ImageUrl!
//    //                    };
//    //                })
//    //                .ToList();
//    //        }

//    //        private List<SizeResponseDto> BuildSizeOptions(List<ProductVariant> productVariants)
//    //        {

//    //            return productVariants
//    //                .GroupBy(v => v.SizeId)
//    //                .Select(g =>
//    //                {
//    //                    var first = g.First();
//    //                    return new SizeResponseDto
//    //                    {
//    //                        SizeId = g.Key,
//    //                        Name = first.Size!.Name!
//    //                    };
//    //                })
//    //                .ToList();
//    //        }

//    //}

//    public class ProductCatalogService : IProductCatalogService
//    {
//        private readonly IProductCatalogRepository _catalogRepository;
//        private readonly IProductVariantRepository _variantRepository;

//        public ProductCatalogService(
//            IProductCatalogRepository catalogRepository,
//            IProductVariantRepository variantRepository)
//        {
//            _catalogRepository = catalogRepository;
//            _variantRepository = variantRepository;
//        }

//        public async Task<ApiResponse<List<ProductVariantResponseDto>>> GetCatalogAsync(
//            ProductVariantQueryDto dto)
//        {
//            var query = new ProductVariantQuery
//            {
//                Search = dto.Search,
//                ProductCategoryId = dto.ProductCategoryId,
//                GenderCategoryId = dto.GenderCategoryId,
//                ColorId = dto.ColorId,
//                SizeId = dto.SizeId,
//                MinPrice = dto.MinPrice,
//                MaxPrice = dto.MaxPrice,
//                SortBy = dto.SortBy,
//                SortOrder = dto.SortOrder,
//                PageNumber = dto.PageNumber,
//                PageSize = dto.PageSize
//            };

//            var (variants, totalCount) =
//                await _catalogRepository.GetVariantsAsync(query);

//            var response = new List<ProductVariantResponseDto>();

//            foreach (var v in variants)
//            {
//                // ✅ SIMPLE STEP
//                var allProductVariants =
//                    await _variantRepository.GetByProductIdAsync(v.ProductId);

//                response.Add(new ProductVariantResponseDto
//                {
//                    ProductVariantId = v.ProductVariantId,

//                    ProductId = v.Product!.ProductId,
//                    ProductName = v.Product.ProductName!,

//                    SizeId = v.SizeId,
//                    Size = v.Size!.Name!,

//                    ColorId = v.ColorId,
//                    Color = v.Color!.ColorName!,
//                    HexCode = v.Color.HexCode,

//                    Price = v.Price,
//                    Stock = v.Stock,
//                    ImageUrl = v.ImageUrl,

//                    // ✅ DISTINCT FROM PRODUCT VARIANTS
//                    AvailableColors = allProductVariants
//                        .GroupBy(x => x.ColorId)
//                        .Select(g =>
//                        {
//                            var first = g.First();
//                            return new ColorResponseDto
//                            {
//                                ColorId = g.Key,
//                                Name = first.Color!.ColorName!,
//                                // using image as preview
//                                HexCode = first.ImageUrl!
//                            };
//                        })
//                        .ToList(),

//                    AvailableSizes = allProductVariants
//                        .GroupBy(x => x.SizeId)
//                        .Select(g =>
//                        {
//                            var first = g.First();
//                            return new SizeResponseDto
//                            {
//                                SizeId = g.Key,
//                                Name = first.Size!.Name!
//                            };
//                        })
//                        .ToList()
//                });
//            }

//            return new ApiResponse<List<ProductVariantResponseDto>>
//            {
//                Success = true,
//                Message = "Products fetched successfully",
//                Data = response,
//                Meta = new
//                {
//                    totalCount,
//                    pageNumber = dto.PageNumber,
//                    pageSize = dto.PageSize
//                }
//            };
//        }
//    }


//}

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
        private readonly IProductVariantRepository _variantRepository;

        public ProductCatalogService(
            IProductCatalogRepository catalogRepository,
            IProductVariantRepository variantRepository)
        {
            _catalogRepository = catalogRepository;
            _variantRepository = variantRepository;
        }

        public async Task<ApiResponse<List<ProductVariantResponseDto>>> GetCatalogAsync(
            ProductVariantQueryDto dto)
        {
            // ================= MAP DTO → QUERY =================
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

            // ================= FETCH VARIANTS (NO PAGINATION) =================
            var (variants, _) = await _catalogRepository.GetVariantsAsync(query);

            // ================= FILTER MODE DETECTION =================
            bool isFilterMode =
                !string.IsNullOrWhiteSpace(dto.Search) ||
                dto.ProductCategoryId?.Any() == true ||
                dto.GenderCategoryId?.Any() == true ||
                dto.ColorId?.Any() == true ||
                dto.SizeId?.Any() == true ||
                dto.MinPrice.HasValue ||
                dto.MaxPrice.HasValue;

            List<ProductVariant> finalVariants;
            int totalCount;

            // ================= DEFAULT vs FILTER MODE =================
            if (!isFilterMode)
            {
                // DEFAULT → ONE VARIANT PER PRODUCT
                finalVariants = variants
                    .GroupBy(v => v.ProductId)
                    .Select(g => g.First())
                    .ToList();

                totalCount = finalVariants.Count;
            }
            else
            {
                // FILTER → ALL VARIANTS
                finalVariants = variants;
                totalCount = variants.Count;
            }

            // ================= PAGINATION (SERVICE LEVEL) =================
            finalVariants = finalVariants
                .Skip((dto.PageNumber - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToList();

            // ================= RESPONSE MAPPING =================
            var response = new List<ProductVariantResponseDto>();

            foreach (var variant in finalVariants)
            {
                // Fetch all variants for this product
                var productVariants =
                    await _variantRepository.GetByProductIdAsync(variant.ProductId);

                response.Add(new ProductVariantResponseDto
                {
                    ProductVariantId = variant.ProductVariantId,

                    ProductId = variant.Product!.ProductId,
                    ProductName = variant.Product.ProductName!,

                    SizeId = variant.SizeId,
                    Size = variant.Size!.Name!,

                    ColorId = variant.ColorId,
                    Color = variant.Color!.ColorName!,
                    HexCode = variant.Color.HexCode,

                    Price = variant.Price,
                    Stock = variant.Stock,
                    ImageUrl = variant.ImageUrl,

                    // DISTINCT OPTIONS (FROM ALL PRODUCT VARIANTS)
                    AvailableColors = BuildColorOptions(productVariants),
                    AvailableSizes = BuildSizeOptions(productVariants)
                });
            }

            // ================= FINAL RESPONSE =================
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

        // ================= HELPER METHODS =================

        private List<ColorResponseDto> BuildColorOptions(
            List<ProductVariant> productVariants)
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
                        // Image per color
                        HexCode = first.ImageUrl!
                    };
                })
                .ToList();
        }

        private List<SizeResponseDto> BuildSizeOptions(
            List<ProductVariant> productVariants)
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
