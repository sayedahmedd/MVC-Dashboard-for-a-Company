using Demo.DAL.Entities;
using Demo.PL.Helper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Reflection.Metadata.Ecma335;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager )
        {
			_userManager = userManager;
			_signInManager = signInManager;
		}

        #region Sign Up
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
		public async Task<IActionResult> SignUp(SignUpViewModel model)
		{
            if(ModelState.IsValid) // Server Side Vaildation
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user is null)
                {
                    user = new ApplicationUser()
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        IsAgree = model.IsAgree ,
                        FName = model.FName ,
                        LName = model.LName 
                        // PasswordHash = model.Password
                    };
                    var result = await _userManager.CreateAsync(user , model.Password);
                    if (result.Succeeded)
                        return RedirectToAction(nameof(SignIn));

                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);


				}
                ModelState.AddModelError(string.Empty , "User name is already Exit");

            }
			return View(model);
		}

        #endregion

        #region Sign In
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
		public async Task<IActionResult> SignIn(SignInViewModel model)
		{
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe ,false );
                        if (result.Succeeded)
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invaild log in");
            }
			return View();
		}

        #endregion

        #region Sign Out
        // Remove Token from Cookies

        public async new Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }

        #endregion

        #region Fotget Password
        public IActionResult FotgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendResetPasswordUrl(FotgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user); // Uniqe for this user for one time 
                var resetPasswordUr1 = Url.Action("ResetPassword", "Account", new { email = model.Email , token = token });
                if (user is not null)
                {
                    var email = new Email()
                    {
                        Subject ="Reset Your Password" ,
                        Recipients = model.Email,
                        Body = resetPasswordUr1
                    };
                    EmailSettings.SendEmail(email);
                    return RedirectToAction(nameof(CheckYourInbox));
				}

				ModelState.AddModelError(string.Empty,"Invalid Email");
			}
            return View(model);

        }

        public IActionResult CheckYourInbox()
        {
            return View();
        }
        #endregion

        #region Reset Password
        public IActionResult ResetPassword(string email, string token)
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModelView model)
        {
            if (ModelState.IsValid)
            {

            var user = await _userManager.FindByEmailAsync(model.email);
            var res = await _userManager.ResetPasswordAsync(user, model.token, model.NewPassword);
            if (res.Succeeded)
                return RedirectToAction(nameof(SignIn)); 

            }
            return View(model);
        }
			#endregion


	}

}
