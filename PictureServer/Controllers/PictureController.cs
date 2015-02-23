using System.Web.Http;
using PictureServer.Models;

namespace PictureServer.Controllers
{
    public class PictureController : ApiController
    {
        [CustomAuthorize(Roles = "Admin")]
        public string Get()
        {
            return new PicturesManager().Get();
        }
    }
}