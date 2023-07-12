using Microsoft.AspNetCore.Identity;

namespace ReadRate.Models
{
    public class UserLogin
    {
        public string UserEmail { get; set; }
        public string Password { get; set; }

        public UserLogin(string userEmail, string password)
        {
            this.UserEmail = userEmail;
            this.Password = password;
        }
    }

    public class UserModel
    {
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public string SecurityQn { get; set; }
        public string SecurityAns { get; set; }
        public Models.Result result { get; set; }

        public UserModel()
        {
            this.result = new Models.Result();
        }
    }

    public class signUpModel
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public string SecurityQn { get; set; }
        public string SecurityAns { get; set; }

    }

    public class loginModel
    {
        public string UserName { get; set; }
    }

    public class UpdateModel
    {
        public int UserId { get; set; }
        public string Password { get; set; }
        public string SecurityQn { get; set; }
        public string SecurityAns { get; set; }
    }

    public class UserDetail
    {
        public int userId { get; set; }
    }

}