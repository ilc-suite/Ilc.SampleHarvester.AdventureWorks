namespace PictureServer.Models
{
    public class Role
    {
        public string RoleName { get; set; }

        public Role(string rolename)
        {
            RoleName = rolename;
        }
    }
}