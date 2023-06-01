using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment2_FrontEnd.Models
{
    public class UserLoginRegister
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        [NotMapped]
        public string Token { get; set; }
        [NotMapped]
        public string Role { get; set; }
    }
}
