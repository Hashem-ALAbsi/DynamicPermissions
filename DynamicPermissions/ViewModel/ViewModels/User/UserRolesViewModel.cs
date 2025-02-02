﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DynamicPermissions.ViewModels.User
{
    public class UserRolesViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public List<string> ValidRoles { get; set; }

        public List<string> SelectedRoles { get; set; }

        public bool IsAdd { get; set; }
    }
}
