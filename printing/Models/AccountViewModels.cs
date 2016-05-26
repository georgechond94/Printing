using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace printing.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(ResourceType = typeof(Resources.Resources), Name = "LoginViewModel_Email_Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(ResourceType = typeof (Resources.Resources), Name = "LoginViewModel_Email_Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "LoginViewModel_Password_Password")]
        public string Password { get; set; }

        [Display(ResourceType = typeof (Resources.Resources), Name = "LoginViewModel_RememberMe_Remember_me_")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(ResourceType = typeof (Resources.Resources), Name = "LoginViewModel_Email_Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof (Resources.Resources), ErrorMessageResourceName = "RegisterViewModel_Password_The__0__must_be_at_least__2__characters_long_", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "LoginViewModel_Password_Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof (Resources.Resources), Name = "RegisterViewModel_ConfirmPassword_Confirm_password")]
        [Compare("Password", ErrorMessageResourceType = typeof (Resources.Resources), ErrorMessageResourceName = "RegisterViewModel_ConfirmPassword_The_password_and_confirmation_password_do_not_match_")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(ResourceType = typeof(Resources.Resources), Name = "LoginViewModel_Email_Email")]
        public string Email { get; set; }
    }
}
