using Assignment2_CurdOperation.Modals;
using Assignment2_CurdOperation.ViewModals;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Assignment2_CurdOperation.Repository.Interface
{
    public interface IStudentVmRepository
    {
        Task<IList<StudentViewModal>> GetStudentViewModals();        
        Task<StudentViewModal> GetStudentById(int id);
        Task<StudentViewModal> GetStudentDetails(int id);
        Task DeleteStudentVM(int id);
        Task CreateStudentVM(StudentViewModal studentvm);
        Task UpdateStudentVM(StudentViewModal studentvm);
       
    }
}
