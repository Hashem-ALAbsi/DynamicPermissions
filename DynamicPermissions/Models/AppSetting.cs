using System;
using System.ComponentModel.DataAnnotations;

namespace DynamicPermissions.Models
{
    public class AppValidationSetting
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime? LastTimeChanged { get; set; }
    }
}
