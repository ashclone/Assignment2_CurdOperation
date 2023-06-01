using Assignment2_FrontEnd.Models;
using Assignment2_FrontEnd.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Assignment2_FrontEnd.Repository
{
    public class StudentVmRepo : Repository<StudentVm>, IStudentVmRepo, T1Interface
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public StudentVmRepo(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> CreateAsync(string url, StudentVm vm, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Headers.Add("Authorization", "Bearer " + token);

            if (vm != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(vm), Encoding.UTF8, "application/json");
            }
            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
                return false;
        }
        

        public async Task<StudentVm?> GetAsync(string url, int id, string token)
        {
           
            var request = new HttpRequestMessage(HttpMethod.Get, url + id.ToString());

            request.Headers.Add("Authorization", "Bearer " + token);

            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<StudentVm>(jsonString);
            }
            return null;
        }

        public async Task<bool> UpdateAsync(string url, StudentVm vm, string token)
        {

            var request = new HttpRequestMessage(HttpMethod.Put, url);

            request.Headers.Add("Authorization", "Bearer " + token);

            if (vm != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(vm), Encoding.UTF8, "application/json");
            }
            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            else
                return false;
        }
        public async Task<bool> DeleteAsync(string url, int id,string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url + id.ToString());
            request.Headers.Add("Authorization", "Bearer " + token);
            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            else
                return false;
        }

        public async Task<bool> DownloadAsync(string url, HttpContext httpContext)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url );            
            var client = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            { 
                var content = await response.Content.ReadAsStreamAsync();
                string contentType = response.Content.Headers.ContentType.ToString();
                var fileName = response.Content.Headers.ContentDisposition.FileName;

                // Download the file to the local file system
                var fileInfo = new FileInfo(Path.Combine(Path.GetTempPath(), fileName));
                using var fileStream = fileInfo.OpenWrite();
                await content.CopyToAsync(fileStream);

                // Return the file to the browser
                httpContext.Response.ContentType = contentType;
                httpContext.Response.Headers.Add("Content-Disposition", $"attachment; filename=\"{fileName}\"");
                await httpContext.Response.SendFileAsync(fileInfo.FullName);
                //File(content, contentType, fileName);

                return true;


            }
            return false;
            
        }

        public Task<bool> DeleteAsync()
        {
           return Task.FromResult(false);
        }
    }
}
