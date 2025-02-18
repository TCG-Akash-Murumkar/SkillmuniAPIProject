using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class CheckNonDisclosureLogController : ApiController
    {
        public HttpResponseMessage Get(int uid, int oid)
        {
            string result = "";

             result = new NonDisclosureLogic().CheckNonDisclosureLog(uid,oid);
          

            return Request.CreateResponse(HttpStatusCode.OK, result);


        }

    }
}
