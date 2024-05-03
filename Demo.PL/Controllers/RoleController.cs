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
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public RoleController(RoleManager<IdentityRole> roleManager , IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                var Roles = await _roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName = R.Name
                    
                }).ToListAsync();
                return View(Roles);
            }
            else
            {
                var Role = await _roleManager.FindByNameAsync(name);
                if (Role is not null)
                {

                    var mappedUser = new RoleViewModel()
                    {
                        Id = Role.Id,
                        RoleName = Role.Name
                    };
                    return View(new List<RoleViewModel>() { mappedUser });
                }
                return View(Enumerable.Empty<RoleViewModel>());
            }
        }
        public async Task<IActionResult>  Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            if(ModelState.IsValid)
            {
                var mapedRole = _mapper.Map<RoleViewModel, IdentityRole>(model);
                await _roleManager.CreateAsync(mapedRole);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        public async Task<IActionResult> Details(string Id, string ValueName = "Details")
        {
            if (Id is null)
                return BadRequest();
            var Role = await _roleManager.FindByIdAsync(Id);

            if (Role is null)
                return NotFound();

            var mappedRole = new RoleViewModel()
            {
                Id = Role.Id,
                RoleName = Role.Name

            };
            return View(mappedRole);
        }
        public async Task<IActionResult> Edit(string Id)
        {
            return await Details(Id, "Edit");
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] string Id, RoleViewModel updateRole)
        {
            if (Id != updateRole.Id)
                return BadRequest();
            try
            {
                var Role = await _roleManager.FindByIdAsync(Id);
                // Role.Id = updateRole.Id;
                Role.Name = updateRole.RoleName;
                await _roleManager.UpdateAsync(Role);
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
            return await Details(Id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string Id, UserViewModel DeleteView)
        {

            try
            {
                var User = await _roleManager.FindByIdAsync(Id);
                await _roleManager.DeleteAsync(User);

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
