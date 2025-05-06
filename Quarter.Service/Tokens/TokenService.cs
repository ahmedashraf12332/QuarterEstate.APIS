using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Quarter.Core.Entities.Identity;
using Quarter.Core.Services.Contract;
namespace Quarter.Service.Tokens
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager)
        {
            try
            {
                var authClaims = new List<Claim>();

                if (!string.IsNullOrWhiteSpace(user.Email))
                    authClaims.Add(new Claim(ClaimTypes.Email, user.Email));
                else
                    throw new ArgumentNullException(nameof(user.Email), "Email is required to create a token");

                authClaims.Add(new Claim(ClaimTypes.GivenName, user.DisplayName ?? "Unknown"));
                authClaims.Add(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? "0000000000"));

                var userRoles = await userManager.GetRolesAsync(user); // هنا احتمال الخطأ
                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["Jwt:DurationInDays"])),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                // اطبع أو رجع الرسالة عشان تعرف فين الغلطة
                throw new Exception("Error generating JWT", ex);
            }
        }

    }
}
