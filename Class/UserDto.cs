using System;

namespace WebApi.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; } 
        public string Password { get; set; }
    }
}