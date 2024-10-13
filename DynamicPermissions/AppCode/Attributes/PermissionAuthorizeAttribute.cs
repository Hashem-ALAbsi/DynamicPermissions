using DynamicPermissions.Data;
using DynamicPermissions.Models;
using DynamicPermissions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DynamicPermissions.App_Code
{
    public class PermissionAuthorizeAttribute : ActionFilterAttribute
    {

        private readonly DynamicPermissionsContext _context;


        public PermissionAuthorizeAttribute( DynamicPermissionsContext context)
        {
            _context = context;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            if (SkipAuthorization(actionDescriptor))
            {
                await base.OnActionExecutionAsync(context, next);
                return;
            }

            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectResult("/Home/AccessDenied");
                return;
            }
            var area = "NoArea";
            var action = actionDescriptor.ActionName;
            var controller = actionDescriptor.ControllerTypeInfo.FullName;
            var actionFullName =area+"|"+ controller + "|" + action;

            var hasPermission = await UserHasPermissionAsync(context.HttpContext.User.FindFirst(ClaimTypes.Role).Value, actionFullName);
            if (!hasPermission)
            {
                context.Result = new RedirectResult("/Home/AccessDenied");
                return;
            }

            await base.OnActionExecutionAsync(context, next);
        }

        private async Task<bool> UserHasPermissionAsync(string? name, string actionFullName)
        {
            var hasPermi = await _context.RoleClaims.AnyAsync(a => a.ClaimValue == actionFullName);
            if (hasPermi)
            {
                return true;
            }
            return false;
        }

        private static bool SkipAuthorization(ControllerActionDescriptor actionDescriptor)
        {
            return actionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        }
    }
}