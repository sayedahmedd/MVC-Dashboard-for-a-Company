using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class SignUpViewModel
	{
        [Required(ErrorMessage = "User Name is required")]
        public string UserName { get; set; }
		[Required(ErrorMessage = "first Name is required")]
		public string FName { get; set; }
		[Required(ErrorMessage = "last Name is required")]
		public string LName { get; set; }

		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage ="Invalid Email")]
		public string Email { get; set; }
		[Required(ErrorMessage = "Password is required")]
		[MinLength(5, ErrorMessage = "Minimum Password Length is")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required(ErrorMessage = "Confirm Password is required")]
		[Compare(nameof(Password), ErrorMessage ="Confirm Password does not match Password")]
		[DataType(DataType.Password)]
		public string Confirmpassword{ get; set; }
		public bool IsAgree { get; set; }

	}
}
