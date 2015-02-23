using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using PictureServer.Models;

namespace PictureServer.Controllers
{
    public class TestController : ApiController
    {
        [CustomAuthorize(Roles = "User")]
        public StatusCodeResult Get()
        {
            return StatusCode(HttpStatusCode.OK);
        }
    }
}