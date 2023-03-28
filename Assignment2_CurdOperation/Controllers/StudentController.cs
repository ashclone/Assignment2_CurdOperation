using Assignment2_CurdOperation.Modals;
using Assignment2_CurdOperation.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Assignment2_CurdOperation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRespository _studentRepository;

        public StudentController(IStudentRespository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Student>> Get()
        {
            return await _studentRepository.GetStudents();

        }
        [HttpGet("{id}")]
        public async Task<Student> Find(int id)
        {
            return await _studentRepository.GetStudentById(id);
        }
        [HttpPost]
        public async Task Post([FromBody] Student student)
        {
            await _studentRepository.AddStudent(student);
        }
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _studentRepository.DeleteStudent(id);
        }
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] Student student)
        {
            await _studentRepository.UpdateStudent(student);
        }
       

    }
}
