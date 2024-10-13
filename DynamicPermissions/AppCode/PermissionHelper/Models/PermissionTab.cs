using System.Collections.Generic;

namespace DynamicPermissions.ViewModels
{
    public class PermissionTab
    {
        public string Name { get; set; }
        public List<PermissionController> Controllers { get; set; } = new List<PermissionController>();
    }
}
