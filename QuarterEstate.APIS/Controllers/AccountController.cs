using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quarter.Core.Dtos.Auth;
using Quarter.Core.Entities.Identity;
using Quarter.Core.Services.Contract;
using Quarter.Service.Tokens;
using QuarterEstate.APIS.Errors;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuarterEstate.APIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(IUserService userService ,ITokenService tokenService, UserManager<AppUser>userManager)
        {
           _userService = userService;
            this._tokenService = tokenService;
            _userManager = userManager;
        }
        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            var user =  await _userService.LoginAsync(loginDto);
            if (user is null)return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized));
            return Ok(user);

        }
        [HttpPost("Regisiter")]
        public async Task<ActionResult> Register(RegisterDto loginDto)
        {
            var user = await _userService.RegisterAsync(loginDto);
            if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest,"Invalid Regsitrration"));
            return Ok(user);

        }
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
             var userEmail= User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null) return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));
            var user= await _userManager.FindByEmailAsync(userEmail);
            if (user is null) return NotFound(new ApiErrorResponse(StatusCodes.Status404NotFound));
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)

            });
        }

    }
}
