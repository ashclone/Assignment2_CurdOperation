using Assignment2_CurdOperation.Data;
using Assignment2_CurdOperation.Modals;
using Assignment2_CurdOperation.Repository.Interface;
using Assignment2_CurdOperation.ViewModals;
using Microsoft.EntityFrameworkCore;

namespace Assignment2_CurdOperation.Repository
{
    public class StudentRepository : IStudentRespository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddStudent(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
        }



        public async Task DeleteStudent(int id)
        {
            var studentInDb = await _context.Students.FindAsync(id);
            if (studentInDb != null)
            {
                _context.Students.Remove(studentInDb);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<Student> GetStudentById(int id)
        {
            var studentInDb = await _context.Students.FindAsync(id);
            if (studentInDb == null) return null;
            return studentInDb;

        }

        public async Task<List<Student>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }



        public async Task UpdateStudent(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }
        
    }
}
