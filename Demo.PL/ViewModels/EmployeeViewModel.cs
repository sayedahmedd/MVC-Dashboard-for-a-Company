using Demo.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public  int Id { get; set; }
        [Required(ErrorMessage = "Name Is Required")]
        public string Name { get; set; }
        [Range(18, 60)]
        public int? Age { get; set; }
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public bool IsACtive { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        
        // [InverseProperty("Employees")]
        public Department department { get; set; }
        public int? DepartmentId { get; set; }

        public IFormFile Image { get; set; }
        public string ImageName { get; set; }
    }
}
