﻿using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class FotgetPasswordViewModel
	{
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Invalid Email")]
		public string Email { get; set; }
	}
}
