using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IDepartmentReposirory _departmentReposirory;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork unitOfWork /*IDepartmentReposirory departmentReposirory*/, IMapper mapper) // Ask Clr for Creating an object from Class Implmenting IDepartmentReposirory
        {
            // _departmentReposirory = departmentReposirory;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var departments = _unitOfWork.DepartmentReposirory.GetAll();
            var mappedDepartment = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewmodel>>(departments);
            ViewData["Message"] = "Saikoo El mtar4m";

            ViewBag.Net = "El 5d3a";
            return View(mappedDepartment);
            
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(DepartmentViewmodel department)
        {
            if(ModelState.IsValid) // Server Side Vaildation
            {
                var mappeddepartment = _mapper.Map<DepartmentViewmodel , Department>(department);
                 _unitOfWork.DepartmentReposirory.Add(mappeddepartment);
                var count = _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }

        public IActionResult Details (int? Id)
        {
            if (Id is null)
                return BadRequest();

            var department = _unitOfWork.DepartmentReposirory.Get(Id.Value);
            var mappedDepartment = _mapper.Map<Department , DepartmentViewmodel>(department);
            if (department is null)
                return NotFound();
                
            return View(mappedDepartment);
        }

        public IActionResult Edit(int? Id)
        {
            if (Id is null)
                return BadRequest();

            var department = _unitOfWork.DepartmentReposirory.Get(Id.Value);
            var mappedDepartment = _mapper.Map<Department , DepartmentViewmodel>(department);

            if (department is null)
                return NotFound();
            return View(mappedDepartment);
        }
        [HttpPost]
        public IActionResult Edit(DepartmentViewmodel department)
        {
            var  mappedDepartment = _mapper.Map<DepartmentViewmodel, Department>(department);
            if(ModelState.IsValid)
            {
                try
                {

                    _unitOfWork.DepartmentReposirory.Update(mappedDepartment);
                    _unitOfWork.Complete();
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception ex)
                {
                    // 1. Log Exception 
                    // 2. outPut Frindely Message
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(mappedDepartment);
        }

        public IActionResult Delete(int? Id)
        {
            if (Id is null)
                return BadRequest();

            var department = _unitOfWork.DepartmentReposirory.Get(Id.Value);
            var mappedDepartment = _mapper.Map<Department, DepartmentViewmodel>(department);

            if (department is null)
                return NotFound();

            return View(mappedDepartment);
        }
        [HttpPost]
        public IActionResult Delete(DepartmentViewmodel department)
        {
            var mappedDepartment = _mapper.Map<DepartmentViewmodel, Department>(department);
            try
            {
                _unitOfWork.DepartmentReposirory.Delete(mappedDepartment);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(mappedDepartment);
            }
        }
    }
}
