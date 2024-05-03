using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.Helper;
using Demo.PL.ViewModels;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;
using System.Reflection.Metadata;

namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentReposirory _departmentRepo;
        private readonly IMapper _mapper;

        public EmployeeController( IUnitOfWork unitOfWork
            /*IEmployeeRepository employeeRepository , IDepartmentReposirory departmentRepo*/ ,
            IMapper mapper) // Ask Clr for Creating an object from Class Implmenting IDepartmentReposirory and IEmployeeRepository
        {
            _unitOfWork = unitOfWork;
            //_employeeRepository = employeeRepository;
            //_departmentRepo = departmentRepo;
            _mapper = mapper;
        }
        public IActionResult Index(string seaechInp)
        {
            if (string.IsNullOrEmpty(seaechInp))
            {
                var employees = _unitOfWork.EmployeeRepository.GetAll();
                var mappedEmp = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
                
                return View(mappedEmp);
            }
            else
            {
                /// Search don not work 
                var Employee = _unitOfWork.EmployeeRepository.SearchByName(seaechInp.ToLower());
                // var mappedEmp = _mapper.Map<IQueryable<Employee>, EmployeeViewModel>(Employee)  ;
                return View(Employee);
            }

        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Departments"]= _unitOfWork.DepartmentReposirory.GetAll();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]  
        // To confirm Action done throw application not outside tool such postman
        public IActionResult Create(EmployeeViewModel employeevm)
        {
            ///1. Maping 
            // Maunal Maping 
            //var mappedEmployee = new Employee()
            //{
            //    Name = employeevm.Name,
            //    Age = employeevm.Age,
            //    Address = employeevm.Address,
            //    Salary = employeevm.Salary,
            //    Email = employeevm.Email,
            //    PhoneNumber = employeevm.PhoneNumber,
            //    IsACtive = employeevm.IsACtive,
            //    HireDate    = employeevm.HireDate,
            //};

            // Casting Maping 

            //Employee mappedEmployee = (Employee) employeevm;

            // we donot use maunal maping we use package called Auto Mapper
            employeevm.ImageName = DocumentSettings.UploadFile(employeevm.Image,"Images");
            
            var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeevm);

            //if (ModelState.IsValid) // Server Side Vaildation
            //{
                _unitOfWork.EmployeeRepository.Add(mappedEmployee);
                var count = _unitOfWork.Complete();
                if (count > 0)
                {

                    // 3. Temp Data
                    TempData["Message"] = "Employee is added Successfully";
                return RedirectToAction(nameof(Index));

            }
            else
                {
                    TempData["Message"] = "An Error with Data Employee not added";

                return RedirectToAction(nameof(Index));

            }
            // };
        }

        public IActionResult Details(int? Id)
        {
            if (Id is null)
                return BadRequest();
            //ViewData["Departments"] = _departmentRepo.Get((int)Id );
            var department = _unitOfWork.EmployeeRepository.Get(Id.Value);
            var mappedEmp = _mapper.Map<Employee,EmployeeViewModel>(department);
            if (department is null)
                return NotFound();

            return View(mappedEmp);
        }

        public IActionResult Edit(int? Id )
        {
            if (Id is null)
                return BadRequest();
            ViewData["Departments"] = _unitOfWork.DepartmentReposirory.GetAll();
            var department = _unitOfWork.EmployeeRepository.Get(Id.Value);
            var mappedEmployee = _mapper.Map<Employee ,EmployeeViewModel >(department);

            if (department is null)
                return NotFound();
            return View(mappedEmployee);
        }
        [HttpPost]
        public IActionResult Edit(EmployeeViewModel employeeVM)
        {
            var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
            ViewData["Departments"] = _unitOfWork.DepartmentReposirory.GetAll();


            try
            {
                    _unitOfWork.EmployeeRepository.Update(mappedEmployee);
                    var count = _unitOfWork.Complete();
                    //if (count>0)
                    //{
                    //    //DocumentSettings.DeleteFile(employeeVM.ImageName, "Images");
                    //    employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image,"Images");
                    //}

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

        public IActionResult Delete(int? Id)
        {

            if (Id is null)
                return BadRequest();

            var employee = _unitOfWork.EmployeeRepository.Get(Id.Value);
            var mappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);

            if (employee is null)
                return NotFound();

            return View(mappedEmployee);
        }
        [HttpPost]
        public IActionResult Delete(EmployeeViewModel employeeVM )
        {

            try
            {
                var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.EmployeeRepository.Delete(mappedEmployee);
                var count = _unitOfWork.Complete();
                if (count > 0)
                    DocumentSettings.DeleteFile(employeeVM.ImageName, "Images");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                return View(mappedEmployee);
            }
        
        }
    }
}
