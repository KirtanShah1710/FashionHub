using ClothStore_Backend.Application.DTOs.RequestDto;
using ClothStore_Backend.Application.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClothStore_Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenderCategoryController : ControllerBase
    {
        private readonly IGenderCategoryService _genderCategoryService;

        public GenderCategoryController(IGenderCategoryService genderCategoryService)
        {
            _genderCategoryService = genderCategoryService;
        }

        // CREATE
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(GenderCategoryDto dto)
        {
            try
            {
                var result = await _genderCategoryService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.GenderCategoryId }, result);
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
            var categories = await _genderCategoryService.GetAllAsync();
            return Ok(categories);
        }

        // GET BY ID
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _genderCategoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound("Gender category not found");

            return Ok(category);
        }

        // UPDATE
        //[Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, GenderCategoryDto dto)
        {
            var updated = await _genderCategoryService.UpdateAsync(id, dto);
            if (!updated)
                return BadRequest("Update failed or category already exists");

            return NoContent();
        }

        // DELETE
        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _genderCategoryService.DeleteAsync(id);
            if (!deleted)
                return NotFound("Gender category not found");

            return NoContent();
        }
    }
}
