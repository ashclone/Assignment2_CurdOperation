using Assignment2_FrontEnd.Models;
using Assignment2_FrontEnd.Repository.Interfaces;

namespace Assignment2_FrontEnd.Repository
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public StudentRepository(IHttpClientFactory httpClientFactory):base(httpClientFactory) 
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
