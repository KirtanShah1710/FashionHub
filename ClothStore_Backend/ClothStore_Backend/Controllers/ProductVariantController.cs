using ClothStore_Backend.Application.DTOs.RequestDto;
using ClothStore_Backend.Application.IService;
using Microsoft.AspNetCore.Mvc;

namespace ClothStore_Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductVariantController : ControllerBase
    {
        private readonly IProductVariantService _productVariantService;

        public ProductVariantController(IProductVariantService productVariantService)
        {
            _productVariantService = productVariantService;
        }

        // ================= GET ALL VARIANTS =================
        // GET: api/product-variant
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var variants = await _productVariantService.GetAllAsync();
            return Ok(variants);
        }

        // ================= GET VARIANT BY ID =================
        // GET: api/product-variant/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var variant = await _productVariantService.GetByIdAsync(id);
            if (variant == null)
                return NotFound("Product variant not found");

            return Ok(variant);
        }

        // ================= CREATE VARIANT =================
        // POST: api/product-variant
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateProductVariantDto dto)
        {
            await _productVariantService.CreateAsync(dto);
            return Ok("Product variant created successfully");
        }

        // ================= UPDATE VARIANT =================
        // PUT: api/product-variant/5
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(int id, [FromForm] CreateProductVariantDto dto)
        {
            var updated = await _productVariantService.UpdateAsync(id, dto);
            if (!updated)
                return NotFound("Product variant not found");

            return Ok("Product variant updated successfully");
        }

        // ================= DELETE VARIANT =================
        // DELETE: api/product-variant/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _productVariantService.DeleteAsync(id);
            if (!deleted)
                return NotFound("Product variant not found");

            return Ok("Product variant deleted successfully");
        }

        // GET: api/product-variant/product/5
        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProductId(int productId)
        {
            var variants = await _productVariantService.GetByProductIdAsync(productId);
            return Ok(variants);
        }

    }
}
