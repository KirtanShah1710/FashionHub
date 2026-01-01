using ClothStore_Backend.Application.DTOs.RequestDto;
using ClothStore_Backend.Application.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClothStore_Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizeController : ControllerBase
    {
        private readonly ISizeService _service;

        public SizeController(ISizeService service)
        {
            _service = service;
        }

        // ================= CREATE =================
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(SizeDto dto)
        {
            try
            {
                var result = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.SizeId }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ================= GET ALL =================
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sizes = await _service.GetAllAsync();
            return Ok(sizes);
        }

        // ================= GET BY ID =================
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var size = await _service.GetByIdAsync(id);
            if (size == null)
                return NotFound("Size not found");

            return Ok(size);
        }

        // ================= UPDATE =================
       // [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, SizeDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (!updated)
                return BadRequest("Update failed or size already exists");

            return NoContent();
        }

        // ================= DELETE =================
       // [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound("Size not found");

            return NoContent();
        }
    }
}
