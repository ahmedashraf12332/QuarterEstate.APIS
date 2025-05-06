using Microsoft.AspNetCore.Identity;
using Quarter.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quarter.Repository.Identity
{
    public static class StoreIdentityDbContextSeed
    {
        public async static Task SeedAppUserAsync(UserManager<AppUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            var adminRole = "Admin";
            var userRole = "User";

            if (!await _roleManager.RoleExistsAsync(adminRole))
                await _roleManager.CreateAsync(new IdentityRole(adminRole));

            if (!await _roleManager.RoleExistsAsync(userRole))
                await _roleManager.CreateAsync(new IdentityRole(userRole));

            var user = new AppUser
            {
                Id = "1",
                Email = "Admin@gmail.com",
                DisplayName = "Admin",
                UserName = "Admin",
                Address = new Address()
                {
                    FName = "Ashraf",
                    LName = "Tiger",
                    Street = "Street 1",
                    City = "Cairo",
                    Country = "Egypt"
                }
            };

            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if (existingUser == null)
            {
                await _userManager.CreateAsync(user, "Ahmed123!");
                await _userManager.AddToRoleAsync(user, adminRole);
            }
        }

    }
}
