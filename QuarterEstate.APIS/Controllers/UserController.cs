using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using Quarter.Core.Dto.Auth;
using Quarter.Core.Dtos.Auth;
using Quarter.Core.Services.Contract;

namespace QuarterEstate.APIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // جلب كل المستخدمين
        
        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            if (!User.IsInRole("Admin"))
                return Forbid("You are not authorized to access this");

            var (users, count) = await _userService.GetAllUsersAsync(pageIndex, pageSize);

            var result = new
            {
                TotalCount = count,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Users = users
            };

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            if (!User.IsInRole("Admin"))
                return Forbid("You are not authorized to access this");

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound("User not found");
            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] RegisterDto dto)
        {
            if (!User.IsInRole("Admin"))
                return Forbid("You are not authorized to add users");

            var createdUser = await _userService.AddUserAsync(dto);
            if (createdUser == null) return BadRequest("User could not be created");
            return Ok(createdUser);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto dto)
        {
            if (!User.IsInRole("Admin"))
                return Forbid("You are not authorized to update user data");

            var result = await _userService.UpdateUserAsync(id, dto);
            if (!result) return NotFound("User not found or update failed");

            return Ok("User updated successfully");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (!User.IsInRole("Admin"))
                return Forbid("You are not authorized to delete users");

            var result = await _userService.DeleteUserAsync(id);
            if (!result) return NotFound("User not found or delete failed");

            return Ok("User deleted successfully");
        }

    }

}

