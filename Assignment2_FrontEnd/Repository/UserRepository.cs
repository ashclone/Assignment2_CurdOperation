using Assignment2_FrontEnd.Models;
using Assignment2_FrontEnd.Repository.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Assignment2_FrontEnd.Repository
{
    public class UserRepository : Repository<UserLoginRegister>, IUserRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public UserRepository(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<UserLoginRegister> Login(string url, UserLoginRegister vm)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (vm != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(vm), Encoding.UTF8, "application/json");                
            }
            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                var jsonString = await response.Content.ReadAsStringAsync();
                var userData = JsonConvert.DeserializeObject<UserLoginRegister>(jsonString);
                return userData;
            }
            else
                return null;
        }
    }
}
