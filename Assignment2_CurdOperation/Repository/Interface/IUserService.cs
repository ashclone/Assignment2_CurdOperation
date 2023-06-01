using Assignment2_CurdOperation.Identity;
using Assignment2_CurdOperation.ViewModals;

namespace Assignment2_CurdOperation.Repository.Interface
{
    public interface IUserService
    {       
         Task<ApplicationUser> Authenticate(LoginViewModel vm);
        Task<ApplicationUser> Register(LoginViewModel vm);
        Task<bool> IsUniqueUser(string userName);
        Task logout();

    }
}
