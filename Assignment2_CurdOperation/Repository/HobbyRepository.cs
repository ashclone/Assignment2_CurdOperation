using Assignment2_CurdOperation.Data;
using Assignment2_CurdOperation.Modals;
using Assignment2_CurdOperation.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Assignment2_CurdOperation.Repository
{
    public class HobbyRepository : IHobbyRepository
    {
        private readonly ApplicationDbContext _context;

        public HobbyRepository(ApplicationDbContext context)
        {
            _context = context;
           
        }

        public async Task<List<Hobby>> GetHobbies()
        {
            return await _context.Hobbies.ToListAsync();

        }
    }
}
