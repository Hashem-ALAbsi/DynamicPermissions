using System.Collections.Generic;

namespace DynamicPermissions.ViewModels
{
    public class PermissionAction
    {
        public string Name { get; set; }
        public bool Selected { get; set; }
        public List<string> FullNames { get; set; }
    }
}
