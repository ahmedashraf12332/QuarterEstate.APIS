using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quarter.Core.ServiceContract;

namespace QuarterEstate.APIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstateController : ControllerBase
    {
        private readonly IProductService _productService;

        public EstateController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllEstate()
        {
            var result = await _productService.GetAllEstatesAsync();
            return Ok(result);
        }
        [HttpGet("EstateLocation")]

        public async Task<IActionResult> GetAllBrandsAsync()
        {
            var result = await _productService.GetAllloctionAsync();
            return Ok(result);
        }
        [HttpGet("Estatetypes")]
        public async Task<IActionResult> GetAllTypesAsync()
        {
            var result = await _productService.GetAllTypeAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int? id)
        {
            if (id is null) return BadRequest("Invalid id !!");
            var result = await _productService.GetEstateById(id.Value);
            if (result is null) return NotFound("the product not found");
            return Ok(result);
        }
    }
}
