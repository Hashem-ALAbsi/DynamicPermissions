using System;

namespace DynamicPermissions.App_Code
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ActionInfoAttribute : Attribute
    {
        public string Name { get; }

        public ActionInfoAttribute(string nameOrGroupName)
        {
            Name = nameOrGroupName;
        }
    }
}
