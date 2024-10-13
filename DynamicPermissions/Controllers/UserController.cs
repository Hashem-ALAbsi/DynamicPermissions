using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using DynamicPermissions.Data;
using DynamicPermissions.Models;
using DynamicPermissions.ViewModel;
using DynamicPermissions.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DynamicPermissions.Controllers
{
    [AllowAnonymous]
    [DisplayName("UserController(just for show)")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly DynamicPermissionsContext _context;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager, DynamicPermissionsContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [DisplayName("Index")]
        public IActionResult Index()
        {
            var model = _userManager.Users.ToList();
            return Ok(model);
        }

        [DisplayName("Add")]
        public IActionResult Add()
        {
            //ViewData["AuthorizorType"] = new SelectList(_context.AuthorizorType, "Id", "AuthoraizationType");

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(InputUserVM vm)
        {
            var user = new ApplicationUser
            {
                UserName = vm.Name, Email = vm.Email,FullName=vm.Name
            };
            var result = await _userManager.CreateAsync(user, vm.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return View();
                }
            }

            return RedirectToAction("Index");
        }

        [DisplayName("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            await _userManager.DeleteAsync(user);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [DisplayName("UpdateSecurityStamp")]
        public async Task<IActionResult> UpdateSecurityStamp(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            await _userManager.UpdateSecurityStampAsync(user);
            return RedirectToAction("Index");
        }

        #region UserRoles

        [HttpGet]
        [DisplayName("UserRoles")]
        public async Task<IActionResult> UserRoles(string id,bool isAdd)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = await _roleManager.Roles.AsNoTracking().ToListAsync();
            var validRoles = isAdd ? roles.Where(r => !userRoles.Contains(r.Name)).Select(role => role.Name).ToList() : userRoles.ToList();

            return View(new UserRolesViewModel
            {
                Id = id,
                UserName = user.UserName,
                IsAdd = isAdd,
                ValidRoles = validRoles
            });
        }

        [HttpPost]
        public async Task<IActionResult> UserRoles(UserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (model.IsAdd)
            {
                await _userManager.AddToRolesAsync(user, model.SelectedRoles);
            }
            else
            {
                await _userManager.RemoveFromRolesAsync(user, model.SelectedRoles);
            }

            return Ok();
        }

        #endregion
    }
}