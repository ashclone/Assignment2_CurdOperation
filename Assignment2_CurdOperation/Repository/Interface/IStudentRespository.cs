using Assignment2_CurdOperation.Modals;

namespace Assignment2_CurdOperation.Repository.Interface
{
    public interface IStudentRespository
    {
        Task<List<Student>> GetStudents();
        Task<Student> GetStudentById(int id);
        Task AddStudent(Student student);
        Task UpdateStudent(Student student);
        Task DeleteStudent(int id);

    }
}
