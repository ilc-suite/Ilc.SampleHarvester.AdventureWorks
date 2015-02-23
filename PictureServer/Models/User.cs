using System.Collections.Generic;

namespace PictureServer.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public List<Role> Roles { get; set; }

        public User(string username, string password, List<Role> roles)
        {
            Username = username;
            Password = password;
            Roles = roles;
        }
    }
}