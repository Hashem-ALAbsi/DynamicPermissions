using System.ComponentModel.DataAnnotations;

namespace DynamicPermissions.ViewModels
{
    public class UserLoginViewModel
    {
        [Display(Name = "UserName")]
        [Required(ErrorMessage = "UserName Is Required")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password Is Required")]
        public string Password { get; set; }
    }
}