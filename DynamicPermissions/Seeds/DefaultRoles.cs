using DynamicPermissions.Data;
using DynamicPermissions.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DynamicPermissions.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager)
        {
            await roleManager.CreateAsync(new Role("Administrator"));
            await roleManager.CreateAsync(new Role("Admin"));
            await roleManager.CreateAsync(new Role("Normal"));
        }
    }
    public static class DefaultData
    {

    }
}