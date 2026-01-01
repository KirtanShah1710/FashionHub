using ClothStore_Backend.Application.DTOs.RequestDto;
using ClothStore_Backend.Application.DTOs.ResponseDto;
using ClothStore_Backend.Application.IService;
using ClothStore_Backend.Data.Entities;
using ClothStore_Backend.Data.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
namespace ClothStore_Backend.Application.Service
{
    public class ProductVariantService : IProductVariantService
    {
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IWebHostEnvironment _env;

        public ProductVariantService(
            IProductVariantRepository productVariantRepository,
            IWebHostEnvironment env)
        {
            _productVariantRepository = productVariantRepository;
            _env = env;
        }

        // ================= GET ALL =================
        public async Task<List<ProductVariantResponseDto>> GetAllAsync()
        {
            var variants = await _productVariantRepository.GetAllAsync();

            return variants.Select(v => new ProductVariantResponseDto
            {
                ProductVariantId = v.ProductVariantId,
                ProductId = v.ProductId,
                ProductName = v.Product!.ProductName!,
                Size = v.Size!.Name!,
                Color = v.Color!.ColorName!,
                HexCode = v.Color.HexCode,
                Price = v.Price,
                Stock = v.Stock,
                ImageUrl = v.ImageUrl
            }).ToList();
        }

        // ================= GET BY ID =================
        public async Task<ProductVariantResponseDto?> GetByIdAsync(int id)
        {
            var variant = await _productVariantRepository.GetByIdAsync(id);
            if (variant == null)
                return null;

            return new ProductVariantResponseDto
            {
                ProductVariantId = variant.ProductVariantId,
                ProductId = variant.ProductId,
                ProductName = variant.Product!.ProductName!,
                Size = variant.Size!.Name!,
                Color = variant.Color!.ColorName!,
                HexCode = variant.Color.HexCode,
                Price = variant.Price,
                Stock = variant.Stock,
                ImageUrl = variant.ImageUrl
            };
        }

        // ================= CREATE =================
        public async Task CreateAsync(CreateProductVariantDto dto)
        {
            string? imageUrl = null;

            if (dto.Image != null)
            {
                imageUrl = await SaveImage(dto.Image, "variants");
            }

            var variant = new ProductVariant
            {
                ProductId = dto.ProductId,
                SizeId = dto.SizeId,
                ColorId = dto.ColorId,
                Price = dto.Price,
                Stock = dto.Stock,
                ImageUrl = imageUrl
            };

            await _productVariantRepository.CreateAsync(variant);
        }

        // ================= UPDATE =================
        public async Task<bool> UpdateAsync(int id, CreateProductVariantDto dto)
        {
            var variant = await _productVariantRepository.GetByIdAsync(id);
            if (variant == null)
                return false;

            if (dto.Image != null)
            {
                variant.ImageUrl = await SaveImage(dto.Image, "variants");
            }

            variant.Price = dto.Price;
            variant.Stock = dto.Stock;

            await _productVariantRepository.UpdateAsync(variant);
            return true;
        }

        // ================= DELETE =================
        public async Task<bool> DeleteAsync(int id)
        {
            var variant = await _productVariantRepository.GetByIdAsync(id);
            if (variant == null)
                return false;

            await _productVariantRepository.DeleteAsync(variant);
            return true;
        }

        // ================= IMAGE HELPER =================
        private async Task<string> SaveImage(IFormFile file, string folder)
        {
            var uploadPath = Path.Combine(_env.WebRootPath, folder);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(uploadPath, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/{folder}/{fileName}";
        }
        public async Task<List<ProductVariantResponseDto>> GetByProductIdAsync(int productId)
        {
            var variants = await _productVariantRepository.GetByProductIdAsync(productId);

            return variants.Select(v => new ProductVariantResponseDto
            {
                ProductVariantId = v.ProductVariantId,

                ProductId = v.ProductId,
                ProductName = v.Product!.ProductName!,

                SizeId = v.SizeId,
                Size = v.Size!.Name!,

                ColorId = v.ColorId,
                Color = v.Color!.ColorName!,
                HexCode = v.Color.HexCode,

                Price = v.Price,
                Stock = v.Stock,
                ImageUrl = v.ImageUrl
            }).ToList();
        }

    }
}
