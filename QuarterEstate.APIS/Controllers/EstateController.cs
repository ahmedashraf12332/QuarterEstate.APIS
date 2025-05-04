using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quarter.Core.Dto;
using Quarter.Core.Helper;
using Quarter.Core.ServiceContract;
using Quarter.Core.Specifications.Estatee;
using QuarterEstate.APIS.Errors;


namespace Quarter.APIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _EstateService;

        public ProductsController(IProductService EstateService)
        {
            _EstateService = EstateService;
        }

        [ProducesResponseType(typeof(PaginationResponse<EstateDto>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<EstateDto>>> GetAllProduct([FromQuery] EstateSpecParams EstateSpec)
        {
            // Adjusted method call to match the signature of IProductService
            var result = await _EstateService.GetAllEstatesAsync();
            return Ok(result);
        }

        [HttpGet("EstateLocation")]
        [ProducesResponseType(typeof(IEnumerable<EstateLocationDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EstateLocationDto>>> GetAllBrands()
        {
            var result = await _EstateService.GetAllloctionAsync();
            return Ok(result);
        }

        [HttpGet("EstateType")]
        [ProducesResponseType(typeof(IEnumerable<EstateDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EstateDto>>> GetAllTypes()
        {
            var result = await _EstateService.GetAllTypeAsync(); // Fixed service reference
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof( EstateDto ), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductById(int? id)
        {
            if (id is null) return BadRequest(new ApiErrorResponse(400));
            var result = await _EstateService.GetEstateById(id.Value); // Fixed service reference
            if (result is null) return NotFound(new ApiErrorResponse(404));
            return Ok(result);
        }
    }
}
