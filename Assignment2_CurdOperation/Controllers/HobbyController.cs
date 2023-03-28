using Assignment2_CurdOperation.Modals;
using Assignment2_CurdOperation.Repository;
using Assignment2_CurdOperation.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment2_CurdOperation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HobbyController : ControllerBase
    {
        private readonly IHobbyRepository _hobbyRepository;

        public HobbyController(IHobbyRepository hobbyRepository)
        {
            _hobbyRepository = hobbyRepository;
        }
        [HttpGet]
        public async Task<IEnumerable<Hobby>> Get()
        {
            return await _hobbyRepository.GetHobbies();

        }
    }
}
