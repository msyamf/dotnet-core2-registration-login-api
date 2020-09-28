using System;
using System.Collections.Generic;

namespace WebApi.Entities
{
    public class User
    {
        public int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Username { get; set; }
        public virtual string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }    
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public List<ProfileUser> ProfileUser { get; set; }
    }

    public class ProfileUser
    {
        public int Id { get; set; }
        public virtual string Kodepos { get; set; }
        public string Alamat { get; set; }
        public virtual string NoTelfon { get; set; }
        public User User { get; set; }
    }
}