using Assignment2_FrontEnd.Models;
using Assignment2_FrontEnd.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

namespace Assignment2_FrontEnd.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Login()
        {
            UserLoginRegister user = new UserLoginRegister();
            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> Login(UserLoginRegister user)
        {

            var result = await _userRepository.Login("https://localhost:7223/api/Account/authenticate", user);
            if (result != null)
            {

                if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
                {
                    HttpContext.Session.SetString("Token", result.Token);
                }

                TempData["success"] = " User Loged in";
            }
            else
            {
                TempData["error"] = "Error while login";
            }

            return RedirectToAction("Index", "StudentVm");

        }
        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }
        [HttpPost]
        [ActionName("Logout")]
        public IActionResult LogoutPost()
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
                {
                    HttpContext.Session.Remove("Token");
                }
                var token = HttpContext.Session.GetString("Token");

                TempData["success"] = "Logout Successfully ";
                return RedirectToAction("Index", "StudentVm");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.ToString();
                return View();
            }
        }
    }
}
