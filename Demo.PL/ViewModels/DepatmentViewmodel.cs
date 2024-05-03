using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
    public class DepartmentViewmodel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Code is Required!")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Name is Required!")]
        public string Name { get; set; }
        [Display(Name = "Data Of Creation")]
        public DateTime CreationDate { get; set; }
    }
}
