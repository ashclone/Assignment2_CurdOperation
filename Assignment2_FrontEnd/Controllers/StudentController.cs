using Assignment2_FrontEnd.Models;
using Assignment2_FrontEnd.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Assignment2_FrontEnd.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;

        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _studentRepository.GetAllAsync("https://localhost:7223/api/Student");
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            Student student = new();
            if (id == null) return View(student);
            student = await _studentRepository.GetAsync("https://localhost:7223/api/Student", id.GetValueOrDefault());
            if (student == null)
            {

                return NotFound();
            }
            return View(student);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Student student)
        {
            if (ModelState.IsValid)
            {
                if (student.Id == 0)
                {
                    await _studentRepository.CreateAsync("https://localhost:7223/api/Student", student);
                    
                }
                else
                {
                    await _studentRepository.UpdateAsync("https://localhost:7223/api/Student", student);
 
                  
                }
                return RedirectToAction("Index");
            }
            else
            {
                
                return View(student);
            }
        }
    }
}
