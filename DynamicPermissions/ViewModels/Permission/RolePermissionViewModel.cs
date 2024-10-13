using System.Collections.Generic;

namespace DynamicPermissions.ViewModels
{
    public class RolePermissionViewModel
    {
        public int RoleId { get; set; }
        public List<string> ActionFullNames { get; set; }
    }
}
