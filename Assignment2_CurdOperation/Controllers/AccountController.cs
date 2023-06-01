using Assignment2_CurdOperation.Repository.Interface;
using Assignment2_CurdOperation.ViewModals;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignment2_CurdOperation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> Autheticate([FromBody] LoginViewModel vm)
        {
            var user = await _userService.Authenticate(vm);
            if (user == null) return BadRequest(new { message = "wrong user/password" });
            return Ok(user);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] LoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var isUniqueUser = _userService.IsUniqueUser(vm.UserName);
                if (!await isUniqueUser)
                    return BadRequest("User Already Exist in Database");
                var userInfo = await _userService.Register(vm);
                if (userInfo == null) return BadRequest("User is Not Created Successfully");
                return Ok("User Created Successfully");
            }
            else
            {
                return BadRequest("Model State is Invalid");
            }
        }
        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _userService.logout();
                return Ok(" User logout .");
            }
            catch
            {
                return BadRequest("Please login first!!");

            }
        }





    }
}
