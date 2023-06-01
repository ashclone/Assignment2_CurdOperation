using Assignment2_FrontEnd.Models;
using Assignment2_FrontEnd.Repository.Interfaces;

namespace Assignment2_FrontEnd.Repository
{
    public class HobbyRepo:Repository<Hobby>, IHobbiesRepo
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HobbyRepo(IHttpClientFactory httpClientFactory):base(httpClientFactory) 
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
