using m2ostnextservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace m2ostnextservice.Controllers
{
    public class GetOfflineExpiryController : ApiController
    {
        public HttpResponseMessage Get(string userid)
        {
            Response response = new Response();

            DateTime expireDate = new OfflineAccess().GetExpiryDate(userid);
            string ret = expireDate.ToString("yyyy-MM-dd");
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }

    }
}
