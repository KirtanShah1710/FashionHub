using ClothStore_Backend.Application.DTOs.RequestDto;
using ClothStore_Backend.Application.DTOs.ResponseDto;
using ClothStore_Backend.Application.IService;
using ClothStore_Backend.Data.Entities;
using ClothStore_Backend.Data.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ClothStore_Backend.Application.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _env;

        public ProductService(IProductRepository productRepository, IWebHostEnvironment env)
        {
            _productRepository = productRepository;
            _env = env;
        }

        public async Task<ProductResponseDto> CreateAsync(CreateProductDto dto)
        {
            var imageUrl = dto.Image != null
                ? await SaveImage(dto.Image, "products")
                : null;

            var product = new Product
            {
                ProductName = dto.ProductName,
                Description = dto.Description,
                ProductImageUrl = imageUrl,
                ProductCategoryId = dto.ProductCategoryId,
                GenderCategoryId = dto.GenderCategoryId
            };

            await _productRepository.CreateAsync(product);
            var created = await _productRepository.GetByIdAsync(product.ProductId);

            return MapToDto(created!);
        }

        public async Task<List<ProductResponseDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(MapToDto).ToList();
        }

        public async Task<ProductResponseDto?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product == null ? null : MapToDto(product);
        }

        public async Task<bool> UpdateAsync(int id, CreateProductDto dto)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return false;

            if (dto.Image != null)
                product.ProductImageUrl = await SaveImage(dto.Image, "products");

            product.ProductName = dto.ProductName;
            product.Description = dto.Description;
            product.ProductCategoryId = dto.ProductCategoryId;
            product.GenderCategoryId = dto.GenderCategoryId;

            await _productRepository.UpdateAsync(product);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return false;

            await _productRepository.DeleteAsync(product);
            return true;
        }

        private ProductResponseDto MapToDto(Product p)
        {
            return new ProductResponseDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Description = p.Description,
                ProductImageUrl = p.ProductImageUrl,
                ProductCategory = p.ProductCategory!.ProCatName!,
                GenderCategory = p.GenderCategory!.GenCatName!
            };
        }

        private async Task<string> SaveImage(IFormFile file, string folder)
        {
            var path = Path.Combine(_env.WebRootPath, folder);
            Directory.CreateDirectory(path);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var fullPath = Path.Combine(path, fileName);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/{folder}/{fileName}";
        }
    }
}
