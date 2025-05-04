using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using Quarter.Core.Dtos.Auth;
using Quarter.Core.Entities.Identity;
using Quarter.Core.Services.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Service.Service.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public UserService(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,ITokenService tokenService)
        {
          _userManager = userManager;
            _signInManager = signInManager;
        _tokenService = tokenService;
        }

      

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
           var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            
                return null;
         var result =await  _signInManager.CheckPasswordSignInAsync(user,loginDto.Password,false);
            if (!result.Succeeded) return null;
            return new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager),

            };
          
        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            if ( await CheckEmailExitsAsync(registerDto.Email)) return null;
            var user =new AppUser()
            {
                Email = registerDto.Email,
                DisplayName = registerDto.DisplayName,
                UserName = registerDto.Email.Split("@")[0],
               
               PhoneNumber = registerDto.PhoneNumber
            };
           var result= await  _userManager.CreateAsync(user,registerDto.Password);
            if(!result.Succeeded) return null;
            return new UserDto() { DisplayName = user.DisplayName, Email = user.Email, Token = await _tokenService.CreateTokenAsync(user, _userManager) };
        }
        public async Task<bool> CheckEmailExitsAsync(string email)
        {
          return await _userManager.FindByEmailAsync(email) is not null;
        }
    }
}
