using Assignment2_CurdOperation.Data;
using Assignment2_CurdOperation.Identity;
using Assignment2_CurdOperation.Modals;
using Assignment2_CurdOperation.Repository.Interface;
using Assignment2_CurdOperation.ViewModals;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Assignment2_CurdOperation.Repository
{
    public class UserServiceRepository : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly ApplicationUserManager _applicationUserManager;
        private readonly ApplicationSignInManager _applicationSignInManager;
        private readonly JwtAppSettingSecretKey _jwtAppSettings;

        public UserServiceRepository(ApplicationUserManager applicationUserManager,
            ApplicationSignInManager applicationSignInManager,
            IOptions<JwtAppSettingSecretKey> jwtAppSettings)
        {
            _applicationUserManager = applicationUserManager;
            _applicationSignInManager = applicationSignInManager;
            _jwtAppSettings = jwtAppSettings.Value;

        }
        public async Task logout()
        {
           await _applicationSignInManager.SignOutAsync();           

        }
        public async Task<bool> IsUniqueUser(string userName)
        {
            var user = await _applicationUserManager.FindByNameAsync(userName);
            if (user == null)
                return true;
            else
                return false;
        }
        public async Task<ApplicationUser> Register(LoginViewModel vm)
        {
            var user = new ApplicationUser();
            user.UserName = vm.UserName;
            user.Email = vm.UserName;
            var UserPassword = vm.Password;
            await _applicationUserManager.CreateAsync(user, UserPassword);
            await _applicationUserManager.AddToRoleAsync(user, "Employee");

            return user;

        }

        public async Task<ApplicationUser> Authenticate(LoginViewModel vm)
        {
            var result = await _applicationSignInManager.PasswordSignInAsync(vm.UserName, vm.Password, false, false);
            if (result.Succeeded)
            {
                var applicationUser = await _applicationUserManager.FindByNameAsync(vm.UserName);

                applicationUser.PasswordHash = "";
                if (await _applicationUserManager.IsInRoleAsync(applicationUser, "Admin"))
                {
                    applicationUser.Role = "Admin";
                }
                if (await _applicationUserManager.IsInRoleAsync(applicationUser, "Employee"))
                {
                    applicationUser.Role = "Employee";
                }

                // JWT 
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtAppSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {

                    new Claim(ClaimTypes.Name, applicationUser.Id.ToString()),
                    new Claim(ClaimTypes.Role, applicationUser.Role)
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                applicationUser.Token = tokenHandler.WriteToken(token);


                return applicationUser;
            }
            return null;
        }

      
    }

}
