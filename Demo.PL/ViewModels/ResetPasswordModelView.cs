using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class ResetPasswordModelView
    {
        [Required(ErrorMessage = "Password is required")]
        [MinLength(5, ErrorMessage = "Minimum Password Length is")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirm Password does not match Password")]
        [DataType(DataType.Password)]
        public string Confirmpassword { get; set; }

        public string token { get; set; }
        public string email { get; set; }
    }
}
