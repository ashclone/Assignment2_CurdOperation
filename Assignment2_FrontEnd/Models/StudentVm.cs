using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Assignment2_FrontEnd.Models
{
    public class StudentVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessage = "The email address is required")]
        [RegularExpression(@"[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",ErrorMessage ="Invalid Email Format")]
        [Display(Name= "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [Range(18, 30, ErrorMessage = "Age must be between 18 and 30")]
        public int Age { get; set; }
        //[DisplayFormat(DataFormatString = "{0:N}",ApplyFormatInEditMode =false)]
        [Range(10000,999999,ErrorMessage ="Salary must be between 10,000 to 9,99,999")]
        public int Salary { get; set; }
        public string Bio { get; set; }
        //public IList<SelectListItem> Hobbies { get; set; } = new HashSet<SelectListItem>().ToList();
        public string[] hob { get; set; }


    }
}
