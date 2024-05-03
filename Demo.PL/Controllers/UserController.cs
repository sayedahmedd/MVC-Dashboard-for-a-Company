using AutoMapper;
using Demo.DAL.Entities;
using Demo.PL.Helper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers
{
    [Authorize(Roles = "Admin")]

    public class UserController : Controller
	{

		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager , IMapper mapper)
		{
			_userManager = userManager;
			_signInManager = signInManager;
            _mapper = mapper;
        }
		public async  Task<IActionResult> Index(string email)
		{
			if (string.IsNullOrEmpty(email))
			{
				var users = await _userManager.Users.Select(U => new UserViewModel()
				{
					Id = U.Id,
					FName = U.FName,
					LName = U.LName,
					Email = U.Email,
					PhoneNumber = U.PhoneNumber,
					Roles = _userManager.GetRolesAsync(U).Result
				}).ToListAsync() ;
				return View(users);
			}
			else
			{
				var user = await _userManager.FindByEmailAsync(email);
				var mappedUser = new UserViewModel()
				{
					Id = user.Id,
					FName = user.FName,
					LName = user.LName,
					Email = user.Email,
					PhoneNumber = user.PhoneNumber,
					Roles = _userManager.GetRolesAsync(user).Result
				};
				return View(new List<UserViewModel>() { mappedUser });
			}
		}
        public async Task<IActionResult> Details(string Id ,string ValueName = "Details")
        {
            if (Id is null)
                return BadRequest();
            //ViewData["Departments"] = _departmentRepo.Get((int)Id );
            var user = await _userManager.FindByIdAsync(Id);
            if (user is null)
                return NotFound();

            var mappeduser = _mapper.Map<ApplicationUser, UserViewModel>(user);
            return View(ValueName,mappeduser);
        }
        public async Task<IActionResult> Edit(string Id )
        {
            
            return await Details(Id, "Edit");
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute]string Id,UserViewModel updateUser)
        {
            if (Id != updateUser.Id)
                return  BadRequest();
            //ViewData["Departments"] = _unitOfWork.DepartmentReposirory.GetAll();


            try
            {
                var user = await _userManager.FindByIdAsync(Id);
                user.FName = updateUser.FName;
                user.LName = updateUser.LName;
                user.PhoneNumber = updateUser.PhoneNumber;
                await _userManager.UpdateAsync(user);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 1. Log Exception 
                // 2. outPut Frindely Message
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View();
        }
        public async Task<IActionResult> DeleteAsync(string Id)
        {
            return await Details(Id , "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string Id,UserViewModel DeleteView)
        {

            try
            {
                var User = await _userManager.FindByIdAsync(Id);
                await _userManager.DeleteAsync(User);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 1. Log Exception 
                // 2. outPut Frindely Message
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View();

        }


    }
}
