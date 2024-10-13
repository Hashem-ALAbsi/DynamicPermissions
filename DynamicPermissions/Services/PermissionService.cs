////using DynamicPermissions.Models;
////using DynamicPermissions.ViewModels;
////using DynamicPermissions.Data;
////using Microsoft.EntityFrameworkCore;
////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Threading.Tasks;

//using DynamicPermissions.Data;

//namespace DynamicPermissions.Services
//{
//    public class PermissionService : IPermissionService
//    {
//        private readonly DynamicPermissionsContext _dbContext;

//        public PermissionService(DynamicPermissionsContext dbContext)
//        {
//            _dbContext = dbContext;
//                   }

//            //        public async Task AddPermissionsIfNotExistsAsync(RolePermissionViewModel model)
//            //        {
//            //            foreach (var action in model.ActionFullNames)
//            //            {
//            //                var permission = new Permission
//            //                {
//            //                    RoleId = model.RoleId,
//            //                    ActionFullName = action,
//            //                };
//            //                var exists = await _dbContext.Permission.AnyAsync(p => p.RoleId == permission.RoleId && p.ActionFullName == permission.ActionFullName);
//            //                if (!exists)
//            //                {
//            //                    _dbContext.Permission.Add(permission);
//            //                    await _dbContext.SaveChangesAsync();
//            //                }
//            //            }
//            //        }

//            //        public async Task DeletePermissionsAsync(RolePermissionViewModel model)
//            //        {
//            //            var permissions = await _dbContext.Permission.Where(p => p.RoleId == model.RoleId && model.ActionFullNames.Contains(p.ActionFullName)).ToListAsync();
//            //            _dbContext.Permission.RemoveRange(permissions);
//            //            await _dbContext.SaveChangesAsync();
//            //        }

//            //        public Task AddRangeAsync(IEnumerable<Permission> permissions)
//            //        {
//            //            _dbContext.Permission.AddRange(permissions);
//            //            return _dbContext.SaveChangesAsync();
//            //        }

//            public async Task<bool> UserHasPermissionAsync(string username, string actionFullName)
//{
//    var userRolesId = await _dbContext.UserRoles.Where(p => p.UserId == username).Select(p => p.RoleId).ToListAsync();
//    var permissions = await _dbContext.Permission.Select(p => new { p.RoleId, p.ActionFullName }).ToListAsync();

//    var hasPermission = permissions.Any(p => userRolesId.Contains(p.RoleId) &&
//         p.ActionFullName.Equals(actionFullName, StringComparison.OrdinalIgnoreCase));

//    return hasPermission;
//}
//    }
//}