using ClothStore_Backend.Application.DTOs.RequestDto;
using ClothStore_Backend.Application.DTOs.ResponseDto;
using ClothStore_Backend.Application.IService;
using ClothStore_Backend.Data.Entities;
using ClothStore_Backend.Data.IRepository;

namespace ClothStore_Backend.Application.Service
{
    public class SizeService : ISizeService
    {
        private readonly ISizeRepository _sizeRepository;

        public SizeService(ISizeRepository sizeRepository)
        {
            _sizeRepository = sizeRepository;
        }

        // CREATE
        public async Task<SizeResponseDto> CreateAsync(SizeDto dto)
        {
            // Check duplicate
            if (await _sizeRepository.ExistsAsync(dto.Name))
                throw new Exception("Size already exists");

            var size = new Size
            {
                Name = dto.Name
            };

            var created = await _sizeRepository.CreateAsync(size);

            return new SizeResponseDto
            {
                SizeId = created.SizeId,
                Name = created.Name!
            };
        }

        // GET ALL
        public async Task<List<SizeResponseDto>> GetAllAsync()
        {
            var sizes = await _sizeRepository.GetAllAsync();

            return sizes.Select(s => new SizeResponseDto
            {
                SizeId = s.SizeId,
                Name = s.Name!
            }).ToList();
        }

        // GET BY ID
        public async Task<SizeResponseDto?> GetByIdAsync(int id)
        {
            var size = await _sizeRepository.GetByIdAsync(id);
            if (size == null)
                return null;

            return new SizeResponseDto
            {
                SizeId = size.SizeId,
                Name = size.Name!
            };
        }

        // UPDATE
        public async Task<bool> UpdateAsync(int id, SizeDto dto)
        {
            var size = await _sizeRepository.GetByIdAsync(id);
            if (size == null)
                return false;

            // Prevent duplicate name
            if (await _sizeRepository.ExistsAsync(dto.Name) &&
                !string.Equals(size.Name, dto.Name, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            size.Name = dto.Name;
            await _sizeRepository.UpdateAsync(size);

            return true;
        }

        // DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            var size = await _sizeRepository.GetByIdAsync(id);
            if (size == null)
                return false;

            await _sizeRepository.DeleteAsync(size);
            return true;
        }
        public async Task<bool> ExistsAsync(string name)
        {
            return await _sizeRepository.ExistsAsync(name);
        }
    }
}
