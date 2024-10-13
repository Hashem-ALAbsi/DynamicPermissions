using DynamicPermissions.Common.Extensions;
using DynamicPermissions.Data;
using DynamicPermissions.Models;
using DynamicPermissions.ViewModels.Role;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DynamicPermissions.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedBasicUserAsync(UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "NormalUser@gmail.com",
                Email = "NormalUser@gmail.com",
                FullName="NormalUser",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
        
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    try
                    {
                        await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                        await userManager.AddToRoleAsync(defaultUser, "Normal");

                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
        }

        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager,DynamicPermissionsContext context)
        {

            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "Administrator@gmail.com",
                Email = "Administrator@gmail.com",
                EmailConfirmed = true,
                FullName="Administrator",
                PhoneNumberConfirmed = true

                
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, "Normal");
                    await userManager.AddToRoleAsync(defaultUser, "Admin");
                    await userManager.AddToRoleAsync(defaultUser, "Administrator");
                }
                await roleManager.SeedClaimsForSuperAdmin();
            }
        }

        private async static Task SeedClaimsForSuperAdmin(this RoleManager<Role> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync("Administrator");
           // var areaViewModels = Assembly.GetEntryAssembly().GetAreaControllerActionNames();
            List<string> roles = new List<string>();
            //foreach (var area in areaViewModels) { }
            //var r=new RoleClaimsViewModel
            //{
            //    //Id = id,
            //    AreaViewModels = areaViewModels
            //};
            var areaViewModel = Assembly.GetEntryAssembly().GetAreaControllerActionNames();
            string x;
            foreach (var area in areaViewModel)
            {
                int commaIndex = area.Area.ToString().IndexOf(':'); // Find the index of the comma

                x = area.Area.ToString().Substring(commaIndex + 1).Trim();
                
                foreach (var con in area.ControllerViewModels)
                {
                    int commaIndexc = con.Controller.ToString().IndexOf(':'); // Find the index of the comma

                   string  cr = con.Controller.ToString().Substring(commaIndexc + 1).Trim();
                    foreach (var act in con.ActionViewModels)
                    {
                        int commaIndexa = act.Action.ToString().IndexOf(':'); // Find the index of the comma

                      string  r=x+ "|" + cr + "|"+ act.Action.ToString().Substring(commaIndexa + 1).Trim();
                        roles.Add(r);
                        //int commaIndexar = act.Action.ToString().IndexOf(':'); // Find the index of the comma

                        //x += "|" + act.Action.ToString().Substring(commaIndexa + 1).Trim();
                    }
                }
            }
            await roleManager.AddPermissionClaim(adminRole,roles);
        }

        public static async Task AddPermissionClaim(this RoleManager<Role> roleManager, Role role, List<string> module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            foreach (var permission in module)
            {
                if (!allClaims.Any(a => a.Type == role.Name && a.Value == permission))
                {
                   // await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                    await roleManager.AddClaimAsync(role, new Claim(role.Name, permission));

                }
            }
        }
    }
}