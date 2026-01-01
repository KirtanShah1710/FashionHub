using ClothStore_Backend.Application.DTOs.RequestDto;
using ClothStore_Backend.Application.DTOs.ResponseDto;
using ClothStore_Backend.Application.IService;
using ClothStore_Backend.Data.Entities;
using ClothStore_Backend.Data.IRepository;

namespace ClothStore_Backend.Application.Service
{
    public class GenderCategoryService : IGenderCategoryService
    {
        private readonly IGenderCategoryRepository _genderCategoryRepository;

        public GenderCategoryService(IGenderCategoryRepository genderCategoryRepository)
        {
            _genderCategoryRepository = genderCategoryRepository;
        }

        // CREATE
        public async Task<GenderCategoryResponseDto> CreateAsync(GenderCategoryDto dto)
        {
            if (await _genderCategoryRepository.ExistsAsync(dto.Name))
                throw new Exception("Gender category already exists");

            var category = new GenderCategory
            {
                GenCatName = dto.Name
            };

            var created = await _genderCategoryRepository.CreateAsync(category);

            return new GenderCategoryResponseDto
            {
                GenderCategoryId = created.GenderCategoryId,
                Name = created.GenCatName!
            };
        }

        // GET ALL
        public async Task<List<GenderCategoryResponseDto>> GetAllAsync()
        {
            var categories = await _genderCategoryRepository.GetAllAsync();

            return categories.Select(g => new GenderCategoryResponseDto
            {
                GenderCategoryId = g.GenderCategoryId,
                Name = g.GenCatName!
            }).ToList();
        }

        // GET BY ID
        public async Task<GenderCategoryResponseDto?> GetByIdAsync(int id)
        {
            var category = await _genderCategoryRepository.GetByIdAsync(id);
            if (category == null)
                return null;

            return new GenderCategoryResponseDto
            {
                GenderCategoryId = category.GenderCategoryId,
                Name = category.GenCatName!
            };
        }

        // UPDATE
        public async Task<bool> UpdateAsync(int id, GenderCategoryDto dto)
        {
            var category = await _genderCategoryRepository.GetByIdAsync(id);
            if (category == null)
                return false;

            if (await _genderCategoryRepository.ExistsAsync(dto.Name) &&
                !string.Equals(category.GenCatName, dto.Name, StringComparison.OrdinalIgnoreCase))
                return false;

            category.GenCatName = dto.Name;
            await _genderCategoryRepository.UpdateAsync(category);

            return true;
        }

        // DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _genderCategoryRepository.GetByIdAsync(id);
            if (category == null)
                return false;

            await _genderCategoryRepository.DeleteAsync(category);
            return true;
        }

        public async Task<bool> ExistsAsync(string name)
        {
            return await _genderCategoryRepository.ExistsAsync(name);
        }
    }
}
