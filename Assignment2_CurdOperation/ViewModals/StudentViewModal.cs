using Assignment2_CurdOperation.Modals;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;

namespace Assignment2_CurdOperation.ViewModals
{
    public class StudentViewModal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public double Salary { get; set; }
        public string Bio { get; set; }
       // public List<SelectListItem> Hobbies { get; set; }
        public string[] hob { get; set; }



    }
}
