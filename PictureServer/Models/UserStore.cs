using System.Collections.Generic;

namespace PictureServer.Models
{
    public class UserStore
    {
        public static List<User> Identities = new List<User>
            {
                new User("test", "test", new List<Role>{ new Role("User") }),
                new User("admin", "admin", new List<Role>{ new Role("Admin"), new Role("User") })
            };
    }
}