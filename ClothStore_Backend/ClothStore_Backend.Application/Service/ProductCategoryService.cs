using ClothStore_Backend.Application.DTOs.RequestDto;
using ClothStore_Backend.Application.DTOs.ResponseDto;
using ClothStore_Backend.Application.IService;
using ClothStore_Backend.Data.Entities;
using ClothStore_Backend.Data.IRepository;

namespace ClothStore_Backend.Application.Service
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoryService(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        public async Task<ProductCategoryResponseDto> CreateAsync(ProductCategoryDto dto)
        {
            if (await _productCategoryRepository.ExistsAsync(dto.Name))
                throw new Exception("Product category already exists");

            var category = new ProductCategory
            {
                ProCatName = dto.Name
            };

            var created = await _productCategoryRepository.CreateAsync(category);

            return new ProductCategoryResponseDto
            {
                ProductCategoryId = created.ProductCategoryId,
                Name = created.ProCatName!
            };
        }

        public async Task<List<ProductCategoryResponseDto>> GetAllAsync()
        {
            var categories = await _productCategoryRepository.GetAllAsync();

            return categories.Select(c => new ProductCategoryResponseDto
            {
                ProductCategoryId = c.ProductCategoryId,
                Name = c.ProCatName!
            }).ToList();
        }

        public async Task<ProductCategoryResponseDto?> GetByIdAsync(int id)
        {
            var category = await _productCategoryRepository.GetByIdAsync(id);
            if (category == null)
                return null;

            return new ProductCategoryResponseDto
            {
                ProductCategoryId = category.ProductCategoryId,
                Name = category.ProCatName!
            };
        }

        public async Task<bool> UpdateAsync(int id, ProductCategoryDto dto)
        {
            var category = await _productCategoryRepository.GetByIdAsync(id);
            if (category == null)
                return false;

            if (await _productCategoryRepository.ExistsAsync(dto.Name) &&
                !string.Equals(category.ProCatName, dto.Name, StringComparison.OrdinalIgnoreCase))
                return false;

            category.ProCatName = dto.Name;
            await _productCategoryRepository.UpdateAsync(category);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _productCategoryRepository.GetByIdAsync(id);
            if (category == null)
                return false;

            await _productCategoryRepository.DeleteAsync(category);
            return true;
        }

        public async Task<bool> ExistsAsync(string name)
        {
            return await _productCategoryRepository.ExistsAsync(name);
        }
    }
}
