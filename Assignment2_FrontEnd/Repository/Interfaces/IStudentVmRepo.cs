using Assignment2_FrontEnd.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment2_FrontEnd.Repository.Interfaces
{
    public interface IStudentVmRepo:IRepository<StudentVm>
    {       
        Task<StudentVm> GetAsync(string url, int id, string token);
        Task<bool> CreateAsync(string url, StudentVm vm, string token);
        Task<bool> UpdateAsync(string url, StudentVm vm, string token);
        Task<bool> DeleteAsync(string url, int id, string token);
        Task<bool> DownloadAsync(string url, HttpContext httpContext);
    }
}
