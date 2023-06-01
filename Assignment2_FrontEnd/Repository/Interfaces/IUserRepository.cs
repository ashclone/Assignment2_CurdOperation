using Assignment2_FrontEnd.Models;

namespace Assignment2_FrontEnd.Repository.Interfaces
{
    public interface IUserRepository:IRepository<UserLoginRegister>
    {
        Task<UserLoginRegister> Login(string url, UserLoginRegister vm);
    }
}
