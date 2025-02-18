using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using m2ostnextservice.Models;

namespace m2ostnextservice.Controllers
{
    public class AuthenticateAPIController : ApiController
    {

        public HttpResponseMessage Get()
        {
            tbl_api_authenticate auth = new tbl_api_authenticate();
            using (m2ostnextserviceDbContext db = new m2ostnextserviceDbContext())
            {
                auth = db.Database.SqlQuery<tbl_api_authenticate>("select * from tbl_api_authenticate").FirstOrDefault();

            }

            return Request.CreateResponse(HttpStatusCode.OK, auth);
            
        }

    }
}
