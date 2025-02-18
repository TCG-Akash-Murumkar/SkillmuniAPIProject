using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class CoroebusConnectController : ApiController
    {
        private db_m2ostEntities db = new db_m2ostEntities();

        public HttpResponseMessage Get(int IDS)
        {
            return Request.CreateResponse(HttpStatusCode.OK, "");
        }
    }
}