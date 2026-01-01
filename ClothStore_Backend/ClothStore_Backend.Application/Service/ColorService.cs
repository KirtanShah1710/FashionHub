using ClothStore_Backend.Application.DTOs.RequestDto;
using ClothStore_Backend.Application.DTOs.ResponseDto;
using ClothStore_Backend.Application.IService;
using ClothStore_Backend.Data.Entities;
using ClothStore_Backend.Data.IRepository;

namespace ClothStore_Backend.Application.Service
{
    public class ColorService : IColorService
    {
        private readonly IColorRepository _colorRepository;

        public ColorService(IColorRepository colorRepository)
        {
            _colorRepository = colorRepository;
        }

        // CREATE
        public async Task<ColorResponseDto> CreateAsync(ColorDto dto)
        {
            if (await _colorRepository.ExistsAsync(dto.Name))
                throw new Exception("Color already exists");

            var color = new Color
            {
                ColorName = dto.Name,
                HexCode = dto.HexCode
            };

            var created = await _colorRepository.CreateAsync(color);

            return new ColorResponseDto
            {
                ColorId = created.ColorId,
                Name = created.ColorName!,
                HexCode = created.HexCode
            };
        }

        // GET ALL
        public async Task<List<ColorResponseDto>> GetAllAsync()
        {
            var colors = await _colorRepository.GetAllAsync();

            return colors.Select(c => new ColorResponseDto
            {
                ColorId = c.ColorId,
                Name = c.ColorName!,
                HexCode = c.HexCode
            }).ToList();
        }

        // GET BY ID
        public async Task<ColorResponseDto?> GetByIdAsync(int id)
        {
            var color = await _colorRepository.GetByIdAsync(id);
            if (color == null)
                return null;

            return new ColorResponseDto
            {
                ColorId = color.ColorId,
                Name = color.ColorName!,
                HexCode = color.HexCode
            };
        }

        // UPDATE
        public async Task<bool> UpdateAsync(int id, ColorDto dto)
        {
            var color = await _colorRepository.GetByIdAsync(id);
            if (color == null)
                return false;

            if (await _colorRepository.ExistsAsync(dto.Name) &&
                !string.Equals(color.ColorName, dto.Name, StringComparison.OrdinalIgnoreCase))
                return false;

            color.ColorName = dto.Name;
            color.HexCode = dto.HexCode;

            await _colorRepository.UpdateAsync(color);
            return true;
        }

        // DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            var color = await _colorRepository.GetByIdAsync(id);
            if (color == null)  
                return false;

            await _colorRepository.DeleteAsync(color);
            return true;
        }

        public async Task<bool> ExistsAsync(string name)
        {
            return await _colorRepository.ExistsAsync(name);
        }
    }
}
