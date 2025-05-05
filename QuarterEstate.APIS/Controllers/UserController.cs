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
       
        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllUsers([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
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


        // جلب مستخدم بالـ ID
        [HttpGet("get-user-by-id/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound("User not found");
            return Ok(user);
        }

        // إضافة مستخدم جديد
        [HttpPost("add-user")]
        public async Task<IActionResult> AddUser([FromBody] RegisterDto dto)
        {
            var createdUser = await _userService.AddUserAsync(dto);
            if (createdUser == null) return BadRequest("User could not be created");
            return Ok(createdUser);
        }

        // تعديل بيانات مستخدم
        [HttpPut("update-user/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto dto)
        {
            var result = await _userService.UpdateUserAsync(id, dto);
            if (!result) return NotFound("User not found or update failed");
            return NoContent();
        }

        // حذف مستخدم
        [HttpDelete("delete-user/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result) return NotFound("User not found or delete failed");
            return NoContent();
        }
    }

}

