using System;

namespace WebApi.Form
{
    public class LoginForm
    { 
        public string username { get; set; }
        public string pasword { get; set; } 
    }

    public class MyProfileFrom
    { 
        public string Alamat { get; set; }
        public string Kodepos { get; set; }
        public string NoTelfon { get; set; } 
    }

        public class UserRegister
    { 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; } 
        public string Password { get; set; }
    }
}