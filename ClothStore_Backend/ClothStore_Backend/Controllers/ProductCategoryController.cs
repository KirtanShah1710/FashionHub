using ClothStore_Backend.Application.DTOs.RequestDto;
using ClothStore_Backend.Application.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClothStore_Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        // CREATE
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(ProductCategoryDto dto)
        {
            try
            {
                var result = await _productCategoryService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.ProductCategoryId }, result);
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
            var categories = await _productCategoryService.GetAllAsync();
            return Ok(categories);
        }

        // GET BY ID
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _productCategoryService.GetByIdAsync(id);
            if (category == null)
                return NotFound("Product category not found");

            return Ok(category);
        }

        // UPDATE
        //[Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductCategoryDto dto)
        {
            var updated = await _productCategoryService.UpdateAsync(id, dto);
            if (!updated)
                return BadRequest("Update failed or category already exists");

            return NoContent();
        }

        // DELETE
        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _productCategoryService.DeleteAsync(id);
            if (!deleted)
                return NotFound("Product category not found");

            return NoContent();
        }
    }
}
