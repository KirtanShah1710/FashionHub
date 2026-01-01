using ClothStore_Backend.Application.DTOs.RequestDto;
using ClothStore_Backend.Application.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClothStore_Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCatalogController : ControllerBase
    {
        private readonly IProductCatalogService _catalogService;

        public ProductCatalogController(
            IProductCatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCatalog(
            [FromQuery] ProductVariantQueryDto query)
        {
            var result = await _catalogService.GetCatalogAsync(query);
            return Ok(result);
        }
    }
}
