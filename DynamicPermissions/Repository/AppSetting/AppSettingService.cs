using DynamicPermissions.Data;
using DynamicPermissions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicPermissions.Repository.AppSetting
{
    public class AppSettingService : IAppSettingService
    {
        private readonly DynamicPermissionsContext _dbContext;
        public AppSettingService(DynamicPermissionsContext context)
        {
            _dbContext = context;
        }

        public string DataBaseRoleValidationGuid()
        {
            var roleValidationGuid = _dbContext.AppValidationSetting.SingleOrDefault(s => s.Key == "RoleValidationGuid")?.Value;

            while (roleValidationGuid == null)
            {
                _dbContext.AppValidationSetting.Add(new AppValidationSetting
                {
                    Key = "RoleValidationGuid",
                    Value = Guid.NewGuid().ToString(),
                    LastTimeChanged = DateTime.Now
                });

                _dbContext.SaveChanges();

                roleValidationGuid = _dbContext.AppValidationSetting.SingleOrDefault(s => s.Key == "RoleValidationGuid")?.Value;
            }

            return roleValidationGuid;
        }
    }
}
