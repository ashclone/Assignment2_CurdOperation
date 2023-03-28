using Assignment2_CurdOperation.Modals;

namespace Assignment2_CurdOperation.Repository.Interface
{
    public interface IHobbyRepository
    {

        Task<List<Hobby>> GetHobbies();
    }
}
