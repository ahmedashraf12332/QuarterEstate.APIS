using Microsoft.AspNetCore.Identity;
using Quarter.Core.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Core.Services.Contract
{
    public interface IUserService
    {
    Task<UserDto>    LoginAsync(LoginDto loginDto);
        Task<UserDto> RegisterAsync(RegisterDto registerDto);
    }
}
