using ClothStore_Backend.Application.DTOs.RequestDto;
using ClothStore_Backend.Application.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClothStore_Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly IColorService _colorService;

        public ColorController(IColorService colorService)
        {
            _colorService = colorService;
        }

        // CREATE
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(ColorDto dto)
        {
            try
            {
                var result = await _colorService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.ColorId }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET ALL
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var colors = await _colorService.GetAllAsync();
            return Ok(colors);
        }

        // GET BY ID
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var color = await _colorService.GetByIdAsync(id);
            if (color == null)
                return NotFound("Color not found");

            return Ok(color);
        }

        // UPDATE
        //[Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ColorDto dto)
        {
            var updated = await _colorService.UpdateAsync(id, dto);
            if (!updated)
                return BadRequest("Update failed or color already exists");

            return NoContent();
        }

        // DELETE
        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _colorService.DeleteAsync(id);
            if (!deleted)
                return NotFound("Color not found");

            return NoContent();
        }
    }
}
