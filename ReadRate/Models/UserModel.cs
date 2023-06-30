﻿using Microsoft.AspNetCore.Identity;

namespace ReadRate.Models
{
    public class UserLogin
    {
        public string UserEmail { get; set; }
        public string Password { get; set; }

    }

    public class UserModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public string SecurityQn { get; set; }
        public string SecurityAns { get; set; }
        public Result result { get; set; }

    }



}