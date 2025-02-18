using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using m2ostnextservice.Models;
namespace m2ostnextservice.Controllers
{
    public class GetMyPlayListController : ApiController
    {
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "");
        }
    }
}
